
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using finalproject.Data;
using finalproject.Models;

namespace finalproject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminContentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminContentController(ApplicationDbContext context)
        {
            _context = context;
        }

     
      
        public async Task<IActionResult> Index()
        {
            var templates = await _context.Templates
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
            return View(templates);
        }

      
        
        public async Task<IActionResult> Detail(int id)
        {
            var template = await _context.Templates
                .Include(t => t.Questions)
                .Include(t => t.Tags)
                .Include(t => t.Comments)
                .Include(t => t.Likes)
                .Include(t => t.FilledForms)
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

          
            template.Title = model.Title;
            template.Description = model.Description;
            template.IsPublic = model.IsPublic;
            template.RestrictedToUserIds = model.RestrictedToUserIds;

            
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploads))
                    Directory.CreateDirectory(uploads);

                var fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(uploads, fileName);
                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(fs);
                }
                template.ImageUrl = "/uploads/" + fileName;
            }

           

            await _context.SaveChangesAsync();
            return RedirectToAction("Dashboard");
        }

        
        
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var template = await _context.Templates.FirstOrDefaultAsync(t => t.Id == id);
            if (template == null)
                return NotFound();
            return View(template);
        }

       
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var template = await _context.Templates.FirstOrDefaultAsync(t => t.Id == id);
            if (template == null)
                return NotFound();

            _context.Templates.Remove(template);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
