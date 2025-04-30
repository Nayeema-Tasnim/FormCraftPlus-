using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using finalproject.Models;

namespace finalproject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: /Admin/Index (User management page)
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var model = new List<UserAdminViewModel>();
            foreach (var user in users)
            {
                bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                model.Add(new UserAdminViewModel
                {
                    Id = user.Id,
                    Email = user.Email ?? string.Empty,
                    UserName = user.UserName ?? string.Empty,
                    IsAdmin = isAdmin,
                    LockoutEnd = user.LockoutEnd?.DateTime
                });
            }
            return View(model);
        }

        // GET: /Admin/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = await _userManager.FindByIdAsync(id!);
            if (user == null)
                return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                Roles = userRoles.ToList(),
                SelectedRoles = userRoles.ToList(),
                AllRoles = _roleManager.Roles.Select(r => r.Name!).ToList()
            };

            return View(model);
        }

        // POST: /Admin/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AllRoles = _roleManager.Roles.Select(r => r.Name!).ToList();
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id!);
            if (user == null)
                return NotFound();

            user.Email = model.Email;
            user.UserName = model.Email;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                model.AllRoles = _roleManager.Roles.Select(r => r.Name!).ToList();
                return View(model);
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var selectedRoles = model.SelectedRoles ?? new List<string>();

            var rolesToRemove = currentRoles.Except(selectedRoles).ToList();
            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

            var rolesToAdd = selectedRoles.Except(currentRoles).ToList();
            await _userManager.AddToRolesAsync(user, rolesToAdd);

            TempData["SuccessMessage"] = "User updated successfully.";
            return RedirectToAction("Index", "Dashboard");
        }

        // GET: /Admin/Delete/{id}
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = await _userManager.FindByIdAsync(id!);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // POST: /Admin/DeleteConfirmed
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id!);
            if (user == null)
                return NotFound();

            var result = await _userManager.DeleteAsync(user);

            TempData["SuccessMessage"] = result.Succeeded ? "User deleted successfully." : "Error deleting user.";
            return RedirectToAction("Index", "Dashboard");
        }

        // GET: /Admin/Block/{id}
        public async Task<IActionResult> Block(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = await _userManager.FindByIdAsync(id!);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // POST: /Admin/BlockConfirmed
        [HttpPost, ActionName("Block")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BlockConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id!);
            if (user == null)
                return NotFound();

            user.LockoutEnabled = true;
            user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(100);

            await _userManager.UpdateAsync(user);

            TempData["SuccessMessage"] = "User blocked successfully.";
            return RedirectToAction("Index", "Dashboard");
        }

        // GET: /Admin/Unblock/{id}
        public async Task<IActionResult> Unblock(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = await _userManager.FindByIdAsync(id!);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // POST: /Admin/UnblockConfirmed
        [HttpPost, ActionName("Unblock")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnblockConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id!);
            if (user == null)
                return NotFound();

            user.LockoutEnd = null;

            await _userManager.UpdateAsync(user);

            TempData["SuccessMessage"] = "User unblocked successfully.";
            return RedirectToAction("Index", "Dashboard");
        }

        // POST: /Admin/GrantAdmin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GrantAdmin(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = await _userManager.FindByIdAsync(id!);
            if (user == null)
                return NotFound();

            if (!await _userManager.IsInRoleAsync(user, "Admin"))
            {
                await _userManager.AddToRoleAsync(user, "Admin");
                TempData["SuccessMessage"] = "Admin role granted successfully.";
            }

            return RedirectToAction("Index", "Dashboard");
        }

        // POST: /Admin/RevokeAdmin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RevokeAdmin(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = await _userManager.FindByIdAsync(id!);
            if (user == null)
                return NotFound();

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                await _userManager.RemoveFromRoleAsync(user, "Admin");
                TempData["SuccessMessage"] = "Admin role revoked successfully.";
            }

            return RedirectToAction("Index", "Dashboard");
        }
    }
}
