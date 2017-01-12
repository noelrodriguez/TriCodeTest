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
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
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
            builder.Entity<OrderOrderedItems>().HasOne(pt => pt.Order).WithMany(t => t.OrderOrderedItems).HasForeignKey(pt => pt.OrderId);
            builder.Entity<OrderOrderedItems>().HasOne(pt => pt.OrderedItem).WithMany(t => t.OrderOrderedItems).HasForeignKey(pt => pt.OrderedItemId);
        }

        public DbSet<Category> Category { get; set; }
        public DbSet<Subcategory> Subcategory { get; set; }
        public DbSet<MenuItem> MenuItem { get; set; }
        public DbSet<Ingredient> Ingredient { get; set; }
        public DbSet<AddOn> AddOn { get; set; }
        public DbSet<MenuItemIngredients> MenuItemIngredients { get; set; }

        public DbSet<Order> Order { get; set; }
        public DbSet<OrderedItem> OrderedItem { get; set; }
        public DbSet<OrderedAddOn> OrderedAddOn { get; set; }
        public DbSet<OrderedIngredient> OrderedIngredient { get; set; }
        public DbSet<OrderOrderedItems> OrderOrderedItems { get; set; }
        public DbSet<OrderedItemAddOns> OrderedItemAddOns { get; set; }
        public DbSet<OrderedItemIngredients> OrderedItemIngredients { get; set; }
    }
}
