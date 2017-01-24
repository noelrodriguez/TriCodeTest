using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TriCodeTest.Models;
using TriCodeTest.Models.MenuModels;
using TriCodeTest.Models.OrderModels;
using Newtonsoft.Json;

namespace TriCodeTest.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
            
            /* Add Order
            var Noel = context.Users.Where(x => x.Email == "noel@gmail.com").FirstOrDefault();

            Order MyOrder = new Order()
            {
                DateTime = System.DateTime.Now,
                Status = Models.Status.Received,
                User = Noel,
            };

            OrderMenuItem MyOrderMenuItem = new OrderMenuItem()
            {
                MenuItem = context.MenuItem.Include(i => i.MenuItemIngredients).Single(m => m.Name == "Banzai Burger"),
                AddOns = new List<AddOn>
                {
                    context.AddOn.Single(a => a.Name == "Bacon"),
                }
            };
            List<OrderMenuItem> TheItems = new List<OrderMenuItem>();
            TheItems.Add(MyOrderMenuItem);
            MyOrder.OrderMenuItems = TheItems;

            double Total = 0;
            foreach (OrderMenuItem i in TheItems)
            {
                Total += i.MenuItem.Price;
                foreach (AddOn j in i.AddOns)
                {
                    Total += j.Price;
                }
            }

            MyOrder.TotalPrice = Total;

            string JSON = JsonConvert.SerializeObject(MyOrder.OrderMenuItems);

            OrderInfo MyOrderInfo = new OrderInfo()
            {
                DateTime = MyOrder.DateTime,
                Status = MyOrder.Status,
                TotalPrice = MyOrder.TotalPrice,
                User = MyOrder.User,
                OrderMenuItems = JSON
            };

            context.OrderInfo.Add(MyOrderInfo);
            context.SaveChanges();

            /*OrderMenuItem AllMenuItems = new OrderMenuItem()
            {
                MenuItem = context.MenuItem.Include(i => i.MenuItemIngredients).Single(m => m.Name == "Cowboy Burger"),
                AddOns = new List<AddOn>
                {
                    context.AddOn.Single(a => a.Name == "Avocado"), context.AddOn.Single(a => a.Name == "Jalapenos")
                }
            };

            double total = AllMenuItems.MenuItem.Price;
            foreach (AddOn ao in AllMenuItems.AddOns)
            {
                total += ao.Price;
            }

            List<OrderMenuItem> ListOfOMI = new List<OrderMenuItem> { AllMenuItems };

            var json = JsonConvert.SerializeObject(ListOfOMI);

            OrderInfo DBOrder = new OrderInfo()
            {
                User = Noel,
                DateTime = System.DateTime.Now,
                Status = Models.Status.Received,
                TotalPrice = total,
                OrderMenuItems = json
            };

            context.OrderInfo.Add(DBOrder);*/


            // Beginning to add a complete set of dummy data

            if (context.Category.Any())
            {
                return;
            }
            // Categories
            // -----------------------------------------------------
            var categories = new Category[]
            {
                new Category { Name = "Appetizers" },
                new Category { Name = "Entrees" }
            };

            foreach (Category c in categories)
            {
                context.Category.Add(c);
            }
            context.SaveChanges();

            // Subcategories
            // -----------------------------------------------------
            var subcategories = new Subcategory[]
            {
                new Subcategory { Name = "Finger Food", CategoryId = categories.Single(i => i.Name == "Appetizers").Id },
                new Subcategory { Name = "Small Portions", CategoryId = categories.Single(i => i.Name == "Appetizers").Id },
                new Subcategory { Name = "Burgers", CategoryId = categories.Single(i => i.Name == "Entrees").Id },
                new Subcategory { Name = "Soups", CategoryId = categories.Single(i => i.Name == "Entrees").Id }
            };

            foreach (Subcategory s in subcategories)
            {
                context.Subcategory.Add(s);
            }
            context.SaveChanges();

            // Addons
            // -----------------------------------------------------
            var addons = new AddOn[]
            {
                new AddOn { Name = "Jalapenos", Price = .75, SubcategoryId = subcategories.Single(i => i.Name == "Burgers").Id },
                new AddOn { Name = "Avocado", Price = .90, SubcategoryId = subcategories.Single(i => i.Name == "Burgers").Id },
                new AddOn { Name = "Pineapple", Price = .75, SubcategoryId = subcategories.Single(i => i.Name == "Burgers").Id },
                new AddOn { Name = "Bacon", Price = 1.25, SubcategoryId = subcategories.Single(i => i.Name == "Burgers").Id },
                new AddOn { Name = "Rice", Price = .50, SubcategoryId = subcategories.Single(i => i.Name == "Soups").Id },
                new AddOn { Name = "Bacon pieces", Price = .75, SubcategoryId = subcategories.Single(i => i.Name == "Soups").Id },
                new AddOn { Name = "Croutons", Price = .75, SubcategoryId = subcategories.Single(i => i.Name == "Soups").Id },
            };

            foreach (AddOn a in addons)
            {
                context.AddOn.Add(a);
            }
            context.SaveChanges();

            // MenuItems
            // -----------------------------------------------------
            var menuitems = new MenuItem[]
            {
                new MenuItem { Name = "Cowboy Burger", Price = 9.99, SubcategoryId = subcategories.Single(i => i.Name == "Burgers").Id },
                new MenuItem { Name = "Wildcat Burger", Price = 11.99, SubcategoryId = subcategories.Single(i => i.Name == "Burgers").Id },
                new MenuItem { Name = "Banzai Burger", Price = 7.99, SubcategoryId = subcategories.Single(i => i.Name == "Burgers").Id },
                new MenuItem { Name = "Bleu Ribbon Burger", Price = 10.99, SubcategoryId = subcategories.Single(i => i.Name == "Burgers").Id },
                new MenuItem { Name = "Tomato Soup", Price = 7.99, SubcategoryId = subcategories.Single(i => i.Name == "Soups").Id },
                new MenuItem { Name = "Zuppa Toscana", Price = 9.99, SubcategoryId = subcategories.Single(i => i.Name == "Soups").Id },
                new MenuItem { Name = "Nachos", Price = 4.99, SubcategoryId = subcategories.Single(i => i.Name == "Finger Food").Id },
                new MenuItem { Name = "Chicken Quesadilla", Price = 5.99, SubcategoryId = subcategories.Single(i => i.Name == "Finger Food").Id },
                new MenuItem { Name = "Fruit Bowl", Price = 2.99, SubcategoryId = subcategories.Single(i => i.Name == "Small Portions").Id },
                new MenuItem { Name = "Greek Yogurt", Price = 2.95, SubcategoryId = subcategories.Single(i => i.Name == "Small Portions").Id },
            };

            foreach (MenuItem m in menuitems)
            {
                context.MenuItem.Add(m);
            }
            context.SaveChanges();

            // Ingredients
            // -----------------------------------------------------
            var ingredients = new Ingredient[]
            {
                new Ingredient { Name = "Mayo", Option = Option.Normal },
                new Ingredient { Name = "Lettuce", Option = Option.Normal },
                new Ingredient { Name = "Tomato", Option = Option.Normal },
                new Ingredient { Name = "Onions", Option = Option.Normal },
                new Ingredient { Name = "Jalapenos", Option = Option.Normal },
                new Ingredient { Name = "Olives", Option = Option.Normal },
                new Ingredient { Name = "Cheddar Cheese", Option = Option.Normal },
                new Ingredient { Name = "Pickles", Option = Option.Normal },
                new Ingredient { Name = "Pineapples", Option = Option.Normal },
                new Ingredient { Name = "Sour Cream", Option = Option.Normal },
                new Ingredient { Name = "Mushrooms", Option = Option.Normal },
                new Ingredient { Name = "Black Beans", Option = Option.Normal },
                new Ingredient { Name = "Bleu Cheese", Option = Option.Normal },
            };

            foreach (Ingredient i in ingredients)
            {
                context.Ingredient.Add(i);
            }
            context.SaveChanges();

            // MenuItemIngredients
            // -----------------------------------------------------
            var menuItemIngredients = new MenuItemIngredients[]
            {
                new MenuItemIngredients {
                    MenuItemId = menuitems.Single(m => m.Name == "Cowboy Burger").Id,
                    IngredientId = ingredients.Single(i => i.Name == "Lettuce").Id
                },
                new MenuItemIngredients {
                    MenuItemId = menuitems.Single(m => m.Name == "Cowboy Burger").Id,
                    IngredientId = ingredients.Single(i => i.Name == "Mayo").Id
                },
                new MenuItemIngredients {
                    MenuItemId = menuitems.Single(m => m.Name == "Cowboy Burger").Id,
                    IngredientId = ingredients.Single(i => i.Name == "Tomato").Id
                },
                new MenuItemIngredients {
                    MenuItemId = menuitems.Single(m => m.Name == "Cowboy Burger").Id,
                    IngredientId = ingredients.Single(i => i.Name == "Onions").Id
                },
                new MenuItemIngredients {
                    MenuItemId = menuitems.Single(m => m.Name == "Cowboy Burger").Id,
                    IngredientId = ingredients.Single(i => i.Name == "Pickles").Id
                },
                new MenuItemIngredients {
                    MenuItemId = menuitems.Single(m => m.Name == "Cowboy Burger").Id,
                    IngredientId = ingredients.Single(i => i.Name == "Cheddar Cheese").Id
                },
                new MenuItemIngredients {
                    MenuItemId = menuitems.Single(m => m.Name == "Wildcat Burger").Id,
                    IngredientId = ingredients.Single(i => i.Name == "Tomato").Id
                },
                new MenuItemIngredients {
                    MenuItemId = menuitems.Single(m => m.Name == "Wildcat Burger").Id,
                    IngredientId = ingredients.Single(i => i.Name == "Lettuce").Id
                },
                new MenuItemIngredients {
                    MenuItemId = menuitems.Single(m => m.Name == "Wildcat Burger").Id,
                    IngredientId = ingredients.Single(i => i.Name == "Jalapenos").Id
                },
                new MenuItemIngredients {
                    MenuItemId = menuitems.Single(m => m.Name == "Wildcat Burger").Id,
                    IngredientId = ingredients.Single(i => i.Name == "Mushrooms").Id
                },
                new MenuItemIngredients {
                    MenuItemId = menuitems.Single(m => m.Name == "Wildcat Burger").Id,
                    IngredientId = ingredients.Single(i => i.Name == "Cheddar Cheese").Id
                },
                new MenuItemIngredients {
                    MenuItemId = menuitems.Single(m => m.Name == "Banzai Burger").Id,
                    IngredientId = ingredients.Single(i => i.Name == "Pineapples").Id
                },
                new MenuItemIngredients {
                    MenuItemId = menuitems.Single(m => m.Name == "Banzai Burger").Id,
                    IngredientId = ingredients.Single(i => i.Name == "Cheddar Cheese").Id
                },
                new MenuItemIngredients {
                    MenuItemId = menuitems.Single(m => m.Name == "Banzai Burger").Id,
                    IngredientId = ingredients.Single(i => i.Name == "Lettuce").Id
                },
                new MenuItemIngredients {
                    MenuItemId = menuitems.Single(m => m.Name == "Banzai Burger").Id,
                    IngredientId = ingredients.Single(i => i.Name == "Tomato").Id
                },
                new MenuItemIngredients {
                    MenuItemId = menuitems.Single(m => m.Name == "Banzai Burger").Id,
                    IngredientId = ingredients.Single(i => i.Name == "Mayo").Id
                },
                new MenuItemIngredients {
                    MenuItemId = menuitems.Single(m => m.Name == "Bleu Ribbon Burger").Id,
                    IngredientId = ingredients.Single(i => i.Name == "Mayo").Id
                },
                new MenuItemIngredients {
                    MenuItemId = menuitems.Single(m => m.Name == "Bleu Ribbon Burger").Id,
                    IngredientId = ingredients.Single(i => i.Name == "Tomato").Id
                },
                new MenuItemIngredients {
                    MenuItemId = menuitems.Single(m => m.Name == "Bleu Ribbon Burger").Id,
                    IngredientId = ingredients.Single(i => i.Name == "Lettuce").Id
                },
                new MenuItemIngredients {
                    MenuItemId = menuitems.Single(m => m.Name == "Bleu Ribbon Burger").Id,
                    IngredientId = ingredients.Single(i => i.Name == "Onions").Id
                },
                new MenuItemIngredients {
                    MenuItemId = menuitems.Single(m => m.Name == "Bleu Ribbon Burger").Id,
                    IngredientId = ingredients.Single(i => i.Name == "Bleu Cheese").Id
                },
                new MenuItemIngredients {
                    MenuItemId = menuitems.Single(m => m.Name == "Chicken Quesadilla").Id,
                    IngredientId = ingredients.Single(i => i.Name == "Jalapenos").Id
                },
                new MenuItemIngredients {
                    MenuItemId = menuitems.Single(m => m.Name == "Chicken Quesadilla").Id,
                    IngredientId = ingredients.Single(i => i.Name == "Olives").Id
                },
                new MenuItemIngredients {
                    MenuItemId = menuitems.Single(m => m.Name == "Chicken Quesadilla").Id,
                    IngredientId = ingredients.Single(i => i.Name == "Onions").Id
                },
                new MenuItemIngredients {
                    MenuItemId = menuitems.Single(m => m.Name == "Chicken Quesadilla").Id,
                    IngredientId = ingredients.Single(i => i.Name == "Tomato").Id
                },
                new MenuItemIngredients {
                    MenuItemId = menuitems.Single(m => m.Name == "Chicken Quesadilla").Id,
                    IngredientId = ingredients.Single(i => i.Name == "Sour Cream").Id
                },
                new MenuItemIngredients {
                    MenuItemId = menuitems.Single(m => m.Name == "Nachos").Id,
                    IngredientId = ingredients.Single(i => i.Name == "Olives").Id
                },
                new MenuItemIngredients {
                    MenuItemId = menuitems.Single(m => m.Name == "Nachos").Id,
                    IngredientId = ingredients.Single(i => i.Name == "Onions").Id
                },
                new MenuItemIngredients {
                    MenuItemId = menuitems.Single(m => m.Name == "Nachos").Id,
                    IngredientId = ingredients.Single(i => i.Name == "Tomato").Id
                },
                new MenuItemIngredients {
                    MenuItemId = menuitems.Single(m => m.Name == "Nachos").Id,
                    IngredientId = ingredients.Single(i => i.Name == "Black Beans").Id
                },
                new MenuItemIngredients {
                    MenuItemId = menuitems.Single(m => m.Name == "Nachos").Id,
                    IngredientId = ingredients.Single(i => i.Name == "Sour Cream").Id
                },
                new MenuItemIngredients {
                    MenuItemId = menuitems.Single(m => m.Name == "Nachos").Id,
                    IngredientId = ingredients.Single(i => i.Name == "Jalapenos").Id
                }
            };

            foreach (MenuItemIngredients m in menuItemIngredients)
            {
                var inDatabase = context.MenuItemIngredients.Where(x => x.Ingredient.Id == m.IngredientId &&
                                                                        x.MenuItem.Id == m.MenuItemId).SingleOrDefault();
                if (inDatabase == null)
                {
                    context.MenuItemIngredients.Add(m);
                }
            }

            context.SaveChanges();
        }
    }
}
