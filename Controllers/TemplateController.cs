
using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using finalproject.Data;
using finalproject.Models;
using finalproject.Services;


namespace finalproject.Controllers
{
    [Authorize]
    public class TemplateController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CloudinaryService _cloudinaryService;

        public TemplateController(ApplicationDbContext context, CloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
         [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Template model, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid)
                return View(model);

            
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploads))
                    Directory.CreateDirectory(uploads);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(uploads, fileName);
                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(fs);
                }
                model.ImageUrl = "/uploads/" + fileName;
            }

            // Set metadata.
            model.CreatedAt = DateTime.UtcNow;
            model.CreatedById = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            // Add the template record.
            _context.Templates.Add(model);
            await _context.SaveChangesAsync();

           
            if (!string.IsNullOrWhiteSpace(model.TagNames))
            {
                var tagNames = model.TagNames.Split(',')
                                             .Select(t => t.Trim())
                                             .Where(t => !string.IsNullOrEmpty(t))
                                             .Distinct();

                foreach (var tagName in tagNames)
                {
                  
                    var tag = await _context.Tags.FirstOrDefaultAsync(x => x.Name.ToLower() == tagName.ToLower());
                    if (tag == null)
                    {
                        tag = new Tag { Name = tagName };
                        _context.Tags.Add(tag);
                        await _context.SaveChangesAsync();
                    }

                 
                    bool relationExists = await _context.TemplateTags
                        .AnyAsync(tt => tt.TemplateId == model.Id && tt.TagId == tag.Id);
                    if (!relationExists)
                    {
                        _context.TemplateTags.Add(new TemplateTag
                        {
                            TemplateId = model.Id,
                            TagId = tag.Id
                        });
                    }
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Dashboard");
        }
    



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuickCreate(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return RedirectToAction("Index", "Home");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            var template = new Template
            {
                Title = title,
                Description = string.Empty,
                IsPublic = true,
                CreatedById = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Templates.Add(template);
            await _context.SaveChangesAsync();

                return RedirectToAction("Create", new { id = template.Id });
        }


        [HttpGet]
          [AllowAnonymous]
   
        public async Task<IActionResult> Detail(int id)
        {
            var template = await _context.Templates
                .Include(t => t.Questions)
                .Include(t => t.Tags)
                .Include(t => t.Comments)
                .Include(t => t.Likes)
                .Include(t => t.FilledForms).ThenInclude(ff => ff.Answers)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (template == null)
                return NotFound();

            return View(template);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var template = await _context.Templates
                .Include(t => t.Questions)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (template == null)
                return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            if (!(User.IsInRole("Admin") || template.CreatedById == userId))
                return Forbid();

            return View(template);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Template model, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid)
                return View(model);

            var template = await _context.Templates
                .Include(t => t.Questions)
                .FirstOrDefaultAsync(t => t.Id == model.Id);

            if (template == null)
                return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            if (!(User.IsInRole("Admin") || template.CreatedById == userId))
                return Forbid();

            template.Title = model.Title;
            template.Description = model.Description;
            template.IsPublic = model.IsPublic;
            template.RestrictedToUserIds = model.RestrictedToUserIds;

            if (ImageFile != null && ImageFile.Length > 0)
            {
                template.ImageUrl = await _cloudinaryService.UploadImageAsync(ImageFile);
            }

            template.Questions = model.Questions;
            await _context.SaveChangesAsync();

            return RedirectToAction("Detail", new { id = template.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Like(int templateId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            var existingLike = await _context.Likes
                .FirstOrDefaultAsync(l => l.TemplateId == templateId && l.UserId == userId);

            if (existingLike == null)
            {
                _context.Likes.Add(new Like {
                    TemplateId = templateId,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                });
            }
            else
            {
                _context.Likes.Remove(existingLike);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Detail", new { id = templateId });
        }

        public async Task<IActionResult> Aggregation(int templateId)
        {
            var aggregationData = await _context.FilledForms
                .Where(ff => ff.TemplateId == templateId)
                .SelectMany(ff => ff.Answers)
                .GroupBy(a => a.QuestionId)
                .Select(g => new {
                    QuestionId = g.Key,
                    AnswerCount = g.Count(),
                    AverageAnswer = g.Average(a => (double?)a.NumericValue)
                })
                .ToListAsync();

            return View(aggregationData);
        }

        
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int templateId, string commentContent)
        {
            if (!await _context.Templates.AnyAsync(t => t.Id == templateId))
            {
                TempData["ErrorMessage"] = "The selected template does not exist.";
                return RedirectToAction("Detail", new { id = templateId });
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "You must be logged in to comment.";
                return RedirectToAction("Detail", new { id = templateId });
            }

            _context.Comments.Add(new Comment {
                TemplateId = templateId,
                Content = commentContent,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();

            return RedirectToAction("Detail", new { id = templateId });
        }
    }
}
