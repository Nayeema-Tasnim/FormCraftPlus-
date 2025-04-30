using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using finalproject.Data;
using finalproject.Models;

namespace finalproject.Controllers
{

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace finalproject.Controllers
{
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }

      
[HttpGet]
public async Task<IActionResult> Search(string query)
{
    ViewData["SearchQuery"] = query ?? "";
    
    if (string.IsNullOrWhiteSpace(query))
        return View(new List<Template>());

    var templates = await _context.Templates
        .Where(t => EF.Functions.Like(t.Title, $"%{query}%")
                 || EF.Functions.Like(t.Description, $"%{query}%"))
        .ToListAsync();

    return View(templates);
}

        }
    }
}



