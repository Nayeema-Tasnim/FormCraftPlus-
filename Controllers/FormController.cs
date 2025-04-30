
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using finalproject.Data;
using finalproject.Models;

using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace finalproject.Controllers
{
    [Authorize]
    public class FormController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FormController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Form/Fill?templateId=...
        [HttpGet]
        public async Task<IActionResult> Fill(int templateId)
        {
            
            var template = await _context.Templates
                .Include(t => t.Questions)
                .FirstOrDefaultAsync(t => t.Id == templateId);

            if (template == null)
            {
                return NotFound();
            }

        
            ViewBag.Template = template;
            return View();
        }

        // POST: /Form/Fill
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Fill(int templateId, IFormCollection form)
        {
           
            var template = await _context.Templates
                .Include(t => t.Questions)
                .FirstOrDefaultAsync(t => t.Id == templateId);
            if (template == null)
                return NotFound();

           
            var filledForm = new FilledForm
            {
                TemplateId = templateId,
                FilledById = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "",
                SubmittedAt = DateTime.Now,
                Answers = new List<Answer>()
            };

        
            foreach (var question in template.Questions)
            {
                string key = "Answer_" + question.Id;
                var response = form[key].ToString();
                if (!string.IsNullOrWhiteSpace(response))
                {
                    filledForm.Answers.Add(new Answer
                    {
                        QuestionId = question.Id,
                        Response = response
                    });
                }
            }

            _context.FilledForms.Add(filledForm);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Dashboard");
        }

        // GET: /Form/Detail/{id}
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            
            var filledForm = await _context.FilledForms
                .Include(f => f.Answers)
                .Include(f => f.Template)
                .FirstOrDefaultAsync(f => f.Id == id);
            if (filledForm == null)
                return NotFound();

            return View(filledForm);
        }
    }
}
