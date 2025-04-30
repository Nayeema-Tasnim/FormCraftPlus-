
//             // Fetch Tags
//             homepageViewModel.Tags = await _context.Tags.OrderBy(t => t.Name).ToListAsync();



using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using finalproject.Data;
using finalproject.Models;

namespace finalproject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext ctx) => _context = ctx;

        public async Task<IActionResult> Index()
        {
            var vm = new HomePageViewModel
            {
                LatestTemplates = await _context.Templates
                                                .OrderByDescending(t => t.CreatedAt)
                                                .Take(10)
                                                .ToListAsync(),

                PopularTemplates = await _context.Templates
                                                 .Select(t => new PopularTemplateViewModel {
                                                     Id       = t.Id,
                                                     Title    = t.Title,
                                                     ImageUrl = t.ImageUrl,
                                                     LikeCount= t.Likes.Count()
                                                 })
                                                 .OrderByDescending(x=>x.LikeCount)
                                                 .Take(10)
                                                 .ToListAsync(),

               // Fetch Tags
               Tags = await _context.Tags.OrderBy(t => t.Name).ToListAsync()
               

            };

            return View(vm);
        }
    }
}
