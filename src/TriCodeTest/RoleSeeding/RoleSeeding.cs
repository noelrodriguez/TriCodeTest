using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TriCodeTest.Data;

namespace TriCodeTest.RoleSeeding
{
    public class RoleSeeding
    {
        //List of roles
        private static readonly string[] Roles = new string[] { "Administrator", "Stuff", "Customer" };

        //Seed roles to the database
        public static async Task SeedRole(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                foreach (var role in Roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role)); //creates
                    }
                }
            }
        }
    }
}
