using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TriCodeTest.Data;
using TriCodeTest.Models.OrderModels;
using Microsoft.EntityFrameworkCore;
using TriCodeTest.Models;



namespace TriCodeTest.Controllers
{
    public class HistoryController : Controller
    {

        //private readonly UserManager<ApplicationUser> _userManager;
        //public HistoryController(UserManager<ApplicationUser> userManager)
        //{
        //    _userManager = userManager;
        //}

        //private readonly ApplicationDbContext _context;

        //public HistoryController(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        //public async Task<IActionResult> Index()
        //{
        //    //var allOrders = await _context.OrderInfo.Include(oi => oi.User).Where(s => s.Status != Models.Status.Cart && s.Status != Models.Status.Completed).ToListAsync();
        //    var userId = User.UserId;

        //    //HistoryViewModel model = new HistoryViewModel()
        //    //{
        //    //    Users = _userManager.Users.ToList(),
        //    //    Orders = _context.OrderInfo.ToList()
        //    //};

        //    //var userId = _userManager
        //    var allOrders = await _context.OrderInfo.ToList();//.Where(o => o.User.Id = UserId);
        //    //var allOrders = await _context.OrderInfo.Include(oi => oi.User).Where(un => un.User.Id == ).ToListAsync();
        //    return View(allOrders);
        //}

        private readonly ApplicationDbContext _context;

        public HistoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            //var allOrders = await _context.OrderInfo.Include(oi => oi.User).Where(s => s.Status != Models.Status.Cart && s.Status != Models.Status.Completed).ToListAsync();
            //var userId = User.UserId;
            //ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            //var allOrders = await _context.OrderInfo.Where(o => o.User.Id = UserId)
            var allOrders = await _context.OrderInfo.ToListAsync();
            return View(allOrders);
        }
    }
}