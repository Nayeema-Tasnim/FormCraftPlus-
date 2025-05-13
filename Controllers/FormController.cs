using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using finalproject.Data;
using finalproject.Models;

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

        [HttpGet]
public async Task<IActionResult> Detail(int id)
{
    var filledForm = await _context.FilledForms
        .Include(f => f.Answers)
        .Include(f => f.Template) 
        .FirstOrDefaultAsync(f => f.Id == id);

    if (filledForm == null)
        return NotFound();

   
    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";


    if (!User.IsInRole("Admin")
        && filledForm.Template.CreatedById != userId
        && filledForm.FilledById != userId)
    {
        return Forbid();
    }

    return View(filledForm);
}


        [HttpGet]
public async Task<IActionResult> Fill(int templateId)
{
  
    var template = await _context.Templates
        .Include(t => t.Questions)
        .FirstOrDefaultAsync(t => t.Id == templateId);
    if (template == null)
        return NotFound();

  
    ViewBag.Template = template;
    return View();
}

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Fill(int templateId, IFormCollection form)
{
    var template = await _context.Templates
        .Include(t => t.Questions)
        .FirstOrDefaultAsync(t => t.Id == templateId);
    if (template == null) return NotFound();

    var ff = new FilledForm {
        TemplateId = templateId,
        FilledById = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "",
        SubmittedAt = DateTime.UtcNow,
        Answers    = new List<Answer>()
    };

    foreach (var q in template.Questions)
    {
        var key = $"Answer_{q.Id}";
        var resp = form[key].FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(resp))
        {
            ff.Answers.Add(new Answer {
                QuestionId = q.Id,
                Response   = resp
            });
        }
    }

    _context.FilledForms.Add(ff);
    await _context.SaveChangesAsync();

    return RedirectToAction("Detail", "Form", new { id = ff.Id });
    }
}}

