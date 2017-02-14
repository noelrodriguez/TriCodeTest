using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TriCodeTest.Models;

namespace TriCodeTest.Controllers
{
    [Produces("application/json")]
    [Route("api/APIUpdateUserRole")]
    public class APIUpdateUserRoleController : Controller
    {
        public void UpdateStatus([FromBody] PostRoleUpdateUserRoleViewModel model)
        {

        }
    }
}