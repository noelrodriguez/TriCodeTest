using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TriCodeTest.Models
{
    public class UpdateUserRoleViewModel
    {
        [Required]
        [EmailAddress]
        public IEnumerable<ApplicationUser> Users { get; set; }
        [Required]
        [EmailAddress]
        public IEnumerable<IdentityRole> Roles { get; set; }
    }
}
