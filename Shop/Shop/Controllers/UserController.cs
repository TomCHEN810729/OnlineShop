using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Shop.Controllers
{
    public class UserController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RoleList()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        public IActionResult RoleCreate()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RoleCreate(IdentityRole role)
        {
            var roleExist = await _roleManager.RoleExistsAsync(role.Name);
            if (!roleExist)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(role.Name));
            }
            return View();
        }

        public IActionResult RoleSend()
        {
            return View();
        }
    }
}
