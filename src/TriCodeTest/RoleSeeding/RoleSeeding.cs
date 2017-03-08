﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TriCodeTest.Data;

namespace TriCodeTest.RoleSeeding
{
    /// <summary>
    /// This class does role seeding during application bootstraping.
    /// Has a function called SeedRole that take a parameter of type
    /// IServiceProvider
    /// </summary>
    public class RoleSeeding
    {
        /// <summary>
        /// List of roles
        /// </summary>
        private static readonly string[] Roles = new string[] { "Admin", "Staff", "Customer" };

        /// <summary>
        /// Method seeds roles in the database for use later
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
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
