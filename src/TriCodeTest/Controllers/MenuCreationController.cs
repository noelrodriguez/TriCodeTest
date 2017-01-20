using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TriCodeTest.Controllers
{
    public class MenuCreationController : Controller
    {
        // GET: /MenuCreation/
        public string Index()
        {
            return "";
        }

        // GET: /MenuCreation/menu
        public IActionResult menu()
        {
            return View();
        }

        //
        // GET: /MenuCreation/Welcome/

        public string Welcome()
        {
            return "This is the Welcome action method...";
        }
    }
}
