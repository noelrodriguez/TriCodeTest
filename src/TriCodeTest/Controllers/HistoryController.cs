using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TriCodeTest.Data;
using TriCodeTest.Models.OrderModels;
using Microsoft.EntityFrameworkCore;

namespace TriCodeTest.Controllers
{
    public class HistoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HistoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            Order oneOrder = new Order();
            var allOrders = _context.OrderInfo.Include(oi => oi.User).Where(s => s.Status != Models.Status.Cart && s.Status != Models.Status.Completed).ToListAsync();
            return View();
        }
    }
}