using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TriCodeTest.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //Add menu item into the cart
        public void AddItem(string cartId, int menuItemId, int quantity)
        {

        }

        //Remove menu item from the cart
        public void RemoveItem(string cartId, int menuItemId)
        {
            //Check the cardId with menuItemId

        }

        //Update current items in the cart
        public void UpdateItem(string cartId, int menuItemId, int quantity)
        {

        }

        public void UpdateShoppingCartDatabase(String cartId, Models.MenuModels.MenuItem[] cartItemsUpdates)
        {
        }
    }
}