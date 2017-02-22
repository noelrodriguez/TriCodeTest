using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TriCodeTest.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TriCodeTest.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TriCodeTest.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UpdateUserRoleController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        public UpdateUserRoleController(
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            UpdateUserRoleViewModel model = new UpdateUserRoleViewModel()
            {
                Users = _userManager.Users.Include(u => u.Roles).ToList(),
                Roles = _roleManager.Roles.ToList()
            };
            var users = _userManager.Users.Include(u => u.Roles).ToList();
            var test = _roleManager.Roles.ToList();
            var roles = _context.Roles.ToList();
            var userRoles = _context.UserRoles.ToList();

            //var roles = _roleManager.Roles.ToList();
            //var roles = _userManager.GetUsersInRoleAsync(users.);

            return View(model);
        }

        public ActionResult UpdateUserRole(FormCollection userId)
        {
            UpdateUserRoleViewModel model = new UpdateUserRoleViewModel()
            {
                Users = _userManager.Users.Include(u => u.Roles).ToList(),
                Roles = _roleManager.Roles.ToList()
            };

            var users = _userManager.Users.Include(u => u.Roles).ToList();
            // prepopulate roles for the view dropdown
            var list = _context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;
            var uList = _context.Users.ToList().Select(rr => new SelectListItem { Value = rr.UserName.ToString(), Text = rr.UserName }).ToList();
            ViewBag.Users = uList;

            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> setRoleAdmin([FromBody] PostRoleUpdateUserRoleViewModel model)
        {
            var roles = _context.Roles.ToList();
            var users = _context.Users.ToList();
            bool successfullyChanged = false;

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var userId = user.Id;
                var oldRoleName = await _userManager.GetRolesAsync(user);

                if(!oldRoleName.Contains("Admin"))
                {
                    await _userManager.RemoveFromRoleAsync(user, oldRoleName[0]);
                    await _userManager.AddToRoleAsync(user, "Admin");
                    successfullyChanged = true;
                }

                _context.Entry(user).State = EntityState.Modified;

                return Content(successfullyChanged.ToString());
            }

            return Content(successfullyChanged.ToString());
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> setRoleStaff([FromBody] PostRoleUpdateUserRoleViewModel model)
        {
            var roles = _context.Roles.ToList();
            var users = _context.Users.ToList();
            bool succcessfullyChanged = false;

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var userId = user.Id;
                var oldRoleName = await _userManager.GetRolesAsync(user);

                if (!oldRoleName.Contains("Staff"))
                {
                    await _userManager.RemoveFromRoleAsync(user, oldRoleName[0]);
                    await _userManager.AddToRoleAsync(user, "Staff");
                    succcessfullyChanged = true;
                }

                _context.Entry(user).State = EntityState.Modified;

                return Content(succcessfullyChanged.ToString());
            }

            return Content(succcessfullyChanged.ToString());
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> setRoleCustomer([FromBody] PostRoleUpdateUserRoleViewModel model)
        {
            var roles = _context.Roles.ToList();
            var users = _context.Users.ToList();
            bool successfullyChanged = false;

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var userId = user.Id;
                var oldRoleName = await _userManager.GetRolesAsync(user);

                if (!oldRoleName.Contains("Customer"))
                {
                    await _userManager.RemoveFromRoleAsync(user, oldRoleName[0]);
                    await _userManager.AddToRoleAsync(user, "Customer");
                    successfullyChanged = true;
                }

                _context.Entry(user).State = EntityState.Modified;

                return Content(successfullyChanged.ToString());
            }

            return Content(successfullyChanged.ToString());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RoleAddToUser(string UserName, string RoleName)
        {
            var users = _userManager.Users.ToList();
            var roles = _context.Roles.ToList();
            var userRoles = _context.UserRoles.ToList();

            if (ModelState.IsValid)
            {
                // THIS LINE IS IMPORTANT
                var oldUser = await _userManager.FindByNameAsync(UserName);
                var oldUserId = oldUser.Id;
                var oldRoleName = await _userManager.GetRolesAsync(oldUser);
                
                if (!oldRoleName.Contains(RoleName))
                {
                    await _userManager.RemoveFromRoleAsync(oldUser, oldRoleName[0]);
                    await _userManager.AddToRoleAsync(oldUser, RoleName);
                }
                _context.Entry(oldUser).State = EntityState.Modified;

                return RedirectToAction("Index", users);
            }

            return RedirectToAction("Index", users);
        }
    }
}
