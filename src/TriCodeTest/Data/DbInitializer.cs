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

            var Noel = context.Users.Where(x => x.Email == "noel@gmail.com");

            if (context.OrderInfo.Any())
            {
                return;
            }

            Order MyOrder = new Order()
            {
                DateTime = System.DateTime.Now,
                Status = Models.Status.Received,
            };

            OrderMenuItem MyOrderMenuItem = new OrderMenuItem()
            {
                MenuItem = new Models.MenuModels.MenuItem()
                {
                    Name = "Cowboy Burger",
                    Price = 9.99,
                },
                AddOns = new List<Models.MenuModels.AddOn>()
                {
                    new Models.MenuModels.AddOn()
                    {
                        Name = "Jalapeno",
                        Price = .50,
                    },
                    new Models.MenuModels.AddOn()
                    {
                        Name = "Avocado",
                        Price = .75,
                    }
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

            string JSON = JsonConvert.SerializeObject(MyOrder);

            OrderInfo MyOrderInfo = new OrderInfo()
            {
                DateTime = MyOrder.DateTime,
                Status = MyOrder.Status,
                TotalPrice = MyOrder.TotalPrice,
                OrderMenuItems = JSON
            };

            context.OrderInfo.Add(MyOrderInfo);

            context.SaveChanges();
        }
    }
}
