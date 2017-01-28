using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TriCodeTest.Controllers
{
    public class AddUserController : Controller
    {
        public IActionResult AddUser()
        {
            return View();
        }
    }
}