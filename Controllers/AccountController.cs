
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using finalproject.Models;
using finalproject.Models.Account;

namespace finalproject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
    
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
    

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }



        
   
        [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Register(RegisterViewModel model)
{
    if (!ModelState.IsValid)
    {
        return View(model);
    }
    
    if (string.IsNullOrWhiteSpace(model.Email))
    {
        ModelState.AddModelError(nameof(model.Email), "Email is required.");
        return View(model);
    }

    if (string.IsNullOrWhiteSpace(model.Password))
    {
        ModelState.AddModelError(nameof(model.Password), "Password is required.");
        return View(model);
    }

    var user = new ApplicationUser
    {
        UserName = model.Email,
        Email = model.Email
    };

    
    var result = await _userManager.CreateAsync(user, model.Password!);
    if (result.Succeeded)
    {
        
        TempData["SuccessMessage"] = "Registration successful! Please log in.";
        return RedirectToAction("Login", "Account");
    }

    foreach (var error in result.Errors)
    {
        ModelState.AddModelError("", error.Description);
    }

    return View(model);
}






[HttpGet]
public IActionResult Login(string? returnUrl = null)
{
    ViewData["ReturnUrl"] = returnUrl;
    return View();
}

// POST: /Account/Login
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
{
    ViewData["ReturnUrl"] = returnUrl;
    if (!ModelState.IsValid)
    {
        return View(model);
    }

    if (string.IsNullOrWhiteSpace(model.Email))
    {
        ModelState.AddModelError(nameof(model.Email), "Email is required.");
        return View(model);
    }

    if (string.IsNullOrWhiteSpace(model.Password))
    {
        ModelState.AddModelError(nameof(model.Password), "Password is required.");
        return View(model);
    }

    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

    if (result.Succeeded)
    {   
        
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user != null && await _userManager.IsInRoleAsync(user, "Admin"))
        {
            
            return RedirectToAction("Index", "Dashboard");
        }
        else
        {
           
            return RedirectToAction("Index", "Dashboard");
        }
    }
    else if (result.IsLockedOut)
    {
        ModelState.AddModelError("", "This account has been locked out. Please try again later.");
        return View(model);
    }
    
   
    ModelState.AddModelError("", "Invalid login attempt.");
    return View(model);
}


    

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
