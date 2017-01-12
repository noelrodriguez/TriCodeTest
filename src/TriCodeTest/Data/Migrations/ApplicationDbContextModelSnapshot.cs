using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using TriCodeTest.Data;

namespace TriCodeTest.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("TriCodeTest.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedUserName")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("TriCodeTest.Models.MenuModels.AddOn", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<double>("Price");

                    b.Property<int>("SubcategoryId");

                    b.HasKey("Id");

                    b.HasIndex("SubcategoryId");

                    b.ToTable("AddOn");
                });

            modelBuilder.Entity("TriCodeTest.Models.MenuModels.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("TriCodeTest.Models.MenuModels.Ingredient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<int>("Option");

                    b.HasKey("Id");

                    b.ToTable("Ingredient");
                });

            modelBuilder.Entity("TriCodeTest.Models.MenuModels.MenuItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<byte[]>("ItemImage");

                    b.Property<string>("Name");

                    b.Property<double>("Price");

                    b.Property<int?>("Size");

                    b.Property<int>("SubcategoryId");

                    b.HasKey("Id");

                    b.HasIndex("SubcategoryId");

                    b.ToTable("MenuItem");
                });

            modelBuilder.Entity("TriCodeTest.Models.MenuModels.MenuItemIngredients", b =>
                {
                    b.Property<int>("MenuItemId");

                    b.Property<int>("IngredientId");

                    b.HasKey("MenuItemId");

                    b.HasIndex("IngredientId");

                    b.HasIndex("MenuItemId");

                    b.ToTable("MenuItemIngredients");
                });

            modelBuilder.Entity("TriCodeTest.Models.MenuModels.Subcategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CategoryId");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Subcategory");
                });

            modelBuilder.Entity("TriCodeTest.Models.OrderModels.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateTime");

                    b.Property<int>("Status");

                    b.Property<double>("TotalPrice");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("TriCodeTest.Models.OrderModels.OrderedAddOn", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<double>("Price");

                    b.Property<int>("Quantity");

                    b.HasKey("Id");

                    b.ToTable("OrderedAddOn");
                });

            modelBuilder.Entity("TriCodeTest.Models.OrderModels.OrderedIngredient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<int>("Option");

                    b.HasKey("Id");

                    b.ToTable("OrderedIngredient");
                });

            modelBuilder.Entity("TriCodeTest.Models.OrderModels.OrderedItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<double>("Price");

                    b.Property<int?>("Size");

                    b.HasKey("Id");

                    b.ToTable("OrderedItem");
                });

            modelBuilder.Entity("TriCodeTest.Models.OrderModels.OrderedItemAddOns", b =>
                {
                    b.Property<int>("OrderedItemId");

                    b.Property<int>("OrderedAddOnId");

                    b.HasKey("OrderedItemId");

                    b.HasIndex("OrderedAddOnId");

                    b.HasIndex("OrderedItemId");

                    b.ToTable("OrderedItemAddOns");
                });

            modelBuilder.Entity("TriCodeTest.Models.OrderModels.OrderedItemIngredients", b =>
                {
                    b.Property<int>("OrderedItemId");

                    b.Property<int>("OrderedIngredientId");

                    b.HasKey("OrderedItemId");

                    b.HasIndex("OrderedIngredientId");

                    b.HasIndex("OrderedItemId");

                    b.ToTable("OrderedItemIngredients");
                });

            modelBuilder.Entity("TriCodeTest.Models.OrderModels.OrderOrderedItems", b =>
                {
                    b.Property<int>("OrderId");

                    b.Property<int>("OrderedItemId");

                    b.HasKey("OrderId");

                    b.HasIndex("OrderId");

                    b.HasIndex("OrderedItemId");

                    b.ToTable("OrderOrderedItems");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("TriCodeTest.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("TriCodeTest.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TriCodeTest.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TriCodeTest.Models.MenuModels.AddOn", b =>
                {
                    b.HasOne("TriCodeTest.Models.MenuModels.Subcategory", "Subcategory")
                        .WithMany("AddOns")
                        .HasForeignKey("SubcategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TriCodeTest.Models.MenuModels.MenuItem", b =>
                {
                    b.HasOne("TriCodeTest.Models.MenuModels.Subcategory", "Subcategory")
                        .WithMany("MenuItems")
                        .HasForeignKey("SubcategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TriCodeTest.Models.MenuModels.MenuItemIngredients", b =>
                {
                    b.HasOne("TriCodeTest.Models.MenuModels.Ingredient", "Ingredient")
                        .WithMany("MenuItemIngredients")
                        .HasForeignKey("IngredientId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TriCodeTest.Models.MenuModels.MenuItem", "MenuItem")
                        .WithMany("MenuItemIngredients")
                        .HasForeignKey("MenuItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TriCodeTest.Models.MenuModels.Subcategory", b =>
                {
                    b.HasOne("TriCodeTest.Models.MenuModels.Category", "Category")
                        .WithMany("Subcategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TriCodeTest.Models.OrderModels.Order", b =>
                {
                    b.HasOne("TriCodeTest.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TriCodeTest.Models.OrderModels.OrderedItemAddOns", b =>
                {
                    b.HasOne("TriCodeTest.Models.OrderModels.OrderedAddOn", "OrderedAddOn")
                        .WithMany("OrderedItemAddOns")
                        .HasForeignKey("OrderedAddOnId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TriCodeTest.Models.OrderModels.OrderedItem", "OrderedItem")
                        .WithMany("OrderedItemAddOns")
                        .HasForeignKey("OrderedItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TriCodeTest.Models.OrderModels.OrderedItemIngredients", b =>
                {
                    b.HasOne("TriCodeTest.Models.OrderModels.OrderedIngredient", "OrderedIngredient")
                        .WithMany("OrderedItemIngredients")
                        .HasForeignKey("OrderedIngredientId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TriCodeTest.Models.OrderModels.OrderedItem", "OrderedItem")
                        .WithMany("OrderedItemIngredients")
                        .HasForeignKey("OrderedItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TriCodeTest.Models.OrderModels.OrderOrderedItems", b =>
                {
                    b.HasOne("TriCodeTest.Models.OrderModels.Order", "Order")
                        .WithMany("OrderOrderedItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TriCodeTest.Models.OrderModels.OrderedItem", "OrderedItem")
                        .WithMany("OrderOrderedItems")
                        .HasForeignKey("OrderedItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
