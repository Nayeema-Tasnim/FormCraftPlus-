

using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using finalproject.Data;
using finalproject.Models;

namespace finalproject.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DashboardController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var model = new DashboardViewModel
            {
                MyTemplates     = await _context.Templates
                                                .Where(t => t.CreatedById == userId)
                                                .OrderByDescending(t => t.CreatedAt)
                                                .ToListAsync(),
                MyFilledForms   = await _context.FilledForms
                                                .Where(f => f.FilledById == userId)
                                                .Include(f=>f.Template)
                                                .OrderByDescending(f=>f.SubmittedAt)
                                                .ToListAsync()
            };
            return View(model);
        }
    }
}
