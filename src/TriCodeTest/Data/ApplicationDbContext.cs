using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TriCodeTest.Models;
using TriCodeTest.Models.MenuModels;
using TriCodeTest.Models.OrderModels;

namespace TriCodeTest.Data
{
    /// <summary>
    /// ApplicationDbContext database class
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext" />
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<MenuItemIngredients>().HasKey(k => new { k.MenuItemId, k.IngredientId });
        }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public DbSet<Category> Category { get; set; }
        /// <summary>
        /// Gets or sets the subcategory.
        /// </summary>
        /// <value>
        /// The subcategory.
        /// </value>
        public DbSet<Subcategory> Subcategory { get; set; }
        /// <summary>
        /// Gets or sets the menu item.
        /// </summary>
        /// <value>
        /// The menu item.
        /// </value>
        public DbSet<MenuItem> MenuItem { get; set; }
        /// <summary>
        /// Gets or sets the ingredient.
        /// </summary>
        /// <value>
        /// The ingredient.
        /// </value>
        public DbSet<Ingredient> Ingredient { get; set; }
        /// <summary>
        /// Gets or sets the add on.
        /// </summary>
        /// <value>
        /// The add on.
        /// </value>
        public DbSet<AddOn> AddOn { get; set; }
        /// <summary>
        /// Gets or sets the menu item ingredients.
        /// </summary>
        /// <value>
        /// The menu item ingredients.
        /// </value>
        public DbSet<MenuItemIngredients> MenuItemIngredients { get; set; }
        /// <summary>
        /// Gets or sets the order information.
        /// </summary>
        /// <value>
        /// The order information.
        /// </value>
        public DbSet<OrderInfo> OrderInfo { get; set; }
    }
}
