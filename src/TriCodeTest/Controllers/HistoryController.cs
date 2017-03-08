using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TriCodeTest.Data;
using TriCodeTest.Models.OrderModels;
using Microsoft.EntityFrameworkCore;
using TriCodeTest.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;


namespace TriCodeTest.Controllers
{
    /// <summary>
    /// Class history
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    public class HistoryController : Controller
    {

        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Order function
        /// </summary>
        /// <value>
        /// The orders.
        /// </value>
        public List<OrderInfo> Orders { get; private set; }
        /// <summary>
        /// Controller
        /// </summary>
        /// <param name="context">context</param>
        public HistoryController(ApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Display list of completed orders.
        /// </summary>
        /// <returns>
        /// Index
        /// </returns>
        //return the view
        public async Task<IActionResult> Index()
        {
            //use userId to select orders submitted by that user
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);//current users id
            var userOrders = await _context.OrderInfo.Where(o => o.User.Id == userId && o.Status == Status.Completed).ToListAsync();

            //return view with the orders selected
            return View(userOrders);
        }
        /// <summary>
        /// Displays order details based on clicked order
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>
        /// View with details for order
        /// </returns>
        public async Task<IActionResult> Details(int? id)
        {
            //return error if no user can be found
            if(id == null)
            {
                return NotFound();
            }//end notFound
            
            //variable to hold the variable that was in the table cell
            var theOrder = await _context.OrderInfo.SingleOrDefaultAsync(i => i.Id == id);

            //return error if no order can be found
            if(theOrder == null)
            {
                return NotFound();
            }//end notFound

            return View(theOrder);
        }
        /// <summary>
        /// Resets the order state to recieved and the DateTime to the current time
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>
        /// Index view with the updated order
        /// </returns>
        [HttpPost]
        public IActionResult resubmitOrder(int? id)
        { 
            HistoryViewModel model = new HistoryViewModel();
            {
                Orders = _context.OrderInfo.ToList();
            };
            if (ModelState.IsValid)
            {
                //cast int as a status to set the status
                int newStat = 1;
                Status newStatus;
                newStatus = (Status)newStat;

                //grab the order in question
                var userOrder = Orders.Where(o => o.Id == id).FirstOrDefault();
                DateTime rightNow = DateTime.Now;
                userOrder.DateTime = rightNow;
                userOrder.Status = newStatus;

                //save changes in try/catch block
                try
                {
                    var userId3 = this.User.FindFirstValue(ClaimTypes.NameIdentifier);//current users id
                    var userOrders3 = _context.OrderInfo.Where(o => o.User.Id == userId3).ToListAsync();
                    _context.SaveChanges();
                    return RedirectToAction("Index", userOrders3);
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }

                //userId and users orders for returning to the index page
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);//current users id
                var userOrders = _context.OrderInfo.Where(o => o.User.Id == userId).ToListAsync();

                return RedirectToAction("Index", userOrders);
            }

            //userId and users orders for returning to the index page
            var userId2 = this.User.FindFirstValue(ClaimTypes.NameIdentifier);//current users id
            var userOrders2 = _context.OrderInfo.Where(o => o.User.Id == userId2).ToListAsync();

            return RedirectToAction("Index", userOrders2);
        }   
    }
}