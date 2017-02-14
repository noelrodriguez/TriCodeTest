using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TriCodeTest.Models
{
    public class UpdateUserRoleViewModel
    {
        public IEnumerable<ApplicationUser> Users { get; set; }
        public IEnumerable<IdentityRole> Roles { get; set; }
    }

    public class PostRoleUpdateUserRoleViewModel
    {
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
