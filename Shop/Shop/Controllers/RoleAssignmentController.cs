using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;

namespace Shop.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class RoleAssignmentController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleAssignmentController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            var roles = _roleManager.Roles.ToList();
            var model = new RoleAssignmentViewModel
            {
                Users = users,
                Roles = roles
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(string selectedUserId, string selectedRoleId)
        {
            var user = await _userManager.FindByIdAsync(selectedUserId);
            if (user == null)
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(selectedRoleId);
            if (role == null)
            {
                return NotFound();
            }

            
            var currentRoles = await _userManager.GetRolesAsync(user);

            
            if (currentRoles.Count > 0)
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeResult.Succeeded)
                {
                    foreach (var error in removeResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View("Assign", new RoleAssignmentViewModel
                    {
                        Users = _userManager.Users.ToList(),
                        Roles = _roleManager.Roles.ToList()
                    });
                }
            }

            
            var result = await _userManager.AddToRoleAsync(user, role.Name);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            var users = _userManager.Users.ToList();
            var roles = _roleManager.Roles.ToList();
            var model = new RoleAssignmentViewModel
            {
                Users = users,
                Roles = roles
            };
            return View("Index", model);
        }

    }
}
