using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TriCodeTest.Models.OrderModels;
using TriCodeTest.Models.MenuModels;
using TriCodeTest.Data;

namespace TriCodeTest.Controllers
{
    public class CartController : Controller
    {
        //List to store order made by customer
        List<OrderMenuItem> menuItem = new List<OrderMenuItem>();
        private readonly ApplicationDbContext _context;

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Add menu item into the cart
        /// </summary>
        /// <param name="item"></param>
        /// <param name="quantity"></param>
        /// MenuItem
        public void AddItem(MenuItem item, int quantity)
        {
            //Grab a menu item and at it to the the orderInfo. (Should be a list)
           /* var currentUserEmail = _context.Users.Where(x => x.Email == "current Email").FirstOrDefault();
            Order MyOrder = new Order()
            {
                DateTime = System.DateTime.Now,
                Status = Models.Status.Received,
                User = currentUserEmail,
            };*/

        }

        /// <summary>
        /// Remove menu item from the cart
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="menuItemId"></param>
        public void RemoveItem(string orderId, int menuItemId)
        {
            //Check the cardId with menuItemId

        }


        /// <summary>
        /// Update current items in the cart
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="menuItemId"></param>
        /// <param name="quantity"></param>
        public void UpdateItem(string orderId, int menuItemId, int quantity)
        {

        }


        /// <summary>
        /// Update order item in the database
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="menuItemUpdate"></param>
        public void UpdateOrderCart(String orderId, MenuItem[] menuItemUpdate)
        {
        }
    }
}