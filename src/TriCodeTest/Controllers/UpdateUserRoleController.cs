using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TriCodeTest.Models;
using Microsoft.AspNetCore.Identity;
using TriCodeTest.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TriCodeTest.Controllers
{
    [Authorize(Roles="Admin")]
    public class UpdateUserRoleController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly RoleManager<ApplicationUser> _roleManager;
        private readonly ApplicationDbContext _context;
        public UpdateUserRoleController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            //_roleManager = roleManager;
            _context = context;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            var roles = _context.Roles.ToList();
            var userRoles = _context.UserRoles.ToList();

            //var list = _context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            //ViewBag.Roles = list;

            //var roles = _roleManager.Roles.ToList();
            //var roles = _userManager.GetUsersInRoleAsync(users.);
            return View(users);
        }

        public ActionResult UpdateUserRole()
        {
            // prepopulat roles for the view dropdown
            var list = _context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;
            var uList = _context.Users.ToList().Select(rr => new SelectListItem { Value = rr.UserName.ToString(), Text = rr.UserName }).ToList();
            ViewBag.Users = uList;
            return View();
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
                //var oldUser = Manager.FindById(userName.Id);
                var oldUser = await _userManager.FindByNameAsync(UserName);
                var oldUserId =  oldUser.Id;
                //var oldRoleId = oldUser.Roles.SingleOrDefault().RoleId;
                var oldRoleName = await _userManager.GetRolesAsync(oldUser);
                //var result = string.Join(", ", oldRoleName.ToArray());
                //var oldRoleName = oldRole.ToString();
                //var oldRole2 = await _roleManager.FindByNameAsync(oldRoleString);
                //var oldRoleName = userRoles.SingleOrDefault(r => r.RoleId == oldRoleId);
                //var oldRoleName = DB.Roles.SingleOrDefault(r => r.Id == oldRoleId).Name;

                if (!oldRoleName.Contains(RoleName))
                {
                    await _userManager.RemoveFromRoleAsync(oldUser, oldRoleName[0]);
                    await _userManager.AddToRoleAsync(oldUser, RoleName);
                }
                _context.Entry(oldUser).State = EntityState.Modified;

                return RedirectToAction("Index", users);
            }

            //ApplicationUser user = _context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            ////var account = new AccountController();
            //_userManager.AddToRoleAsync(user, RoleName);

            //ViewBag.ResultMessage = "Role created successfully !";

            //// prepopulat roles for the view dropdown
            //var list = _context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            //ViewBag.Roles = list;

            return RedirectToAction("Index", users);
        }
    }
}