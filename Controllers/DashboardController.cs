// // using System.Linq;
// // using System.Security.Claims;
// // using System.Threading.Tasks;
// // using Microsoft.AspNetCore.Authorization;
// // using Microsoft.AspNetCore.Mvc;
// // using Microsoft.EntityFrameworkCore;
// // using finalproject.Data;
// // using finalproject.Models;

// // namespace finalproject.Controllers
// // {
// //     [Authorize]
// //     public class DashboardController : Controller
// //     {
// //          private readonly ApplicationDbContext _context;
// //          public DashboardController(ApplicationDbContext context)
// //          {
// //               _context = context;
// //          }

       
// //          public async Task<IActionResult> Index()
// //         {
// //             string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
// //             var myTemplates = await _context.Templates
// //                 .Where(t => t.CreatedById == userId)
// //                 .OrderByDescending(t => t.CreatedAt)
// //                 .ToListAsync();

// //             return View(myTemplates);
// //     }
// // }}




// using System.Linq;
// using System.Security.Claims;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using finalproject.Data;
// using finalproject.Models;

// namespace finalproject.Controllers
// {
//     [Authorize]
//     public class DashboardController : Controller
//     {
//         private readonly ApplicationDbContext _context;

//         public DashboardController(ApplicationDbContext context)
//         {
//             _context = context;
//         }

//         public async Task<IActionResult> Index()
//         {
//             // Get current user ID
//             string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

//             // Fetch templates created by this user, most recent first,
//             // including Questions, Likes and Tags for display.
//             var myTemplates = await _context.Templates
//                 .Where(t => t.CreatedById == userId)
//                 .Include(t => t.Questions)
//                 .Include(t => t.Likes)
//                 .Include(t => t.Tags)
//                 .OrderByDescending(t => t.CreatedAt)
//                 .ToListAsync();

//             // Fetch forms this user has filled, most recent first,
//             // including the Template for title display.
//             var myFilledForms = await _context.FilledForms
//                 .Where(f => f.FilledById == userId)
//                 .Include(f => f.Template)
//                 .OrderByDescending(f => f.SubmittedAt)
//                 .ToListAsync();

//             // Populate the view model
//             var model = new DashboardViewModel
//             {
//                 MyTemplates   = myTemplates,
//                 MyFilledForms = myFilledForms
//             };

//             return View(model);
//         }
//     }
// }


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
