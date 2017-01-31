using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TriCodeTest.Data;
using TriCodeTest.Models.OrderModels;
using Newtonsoft.Json;

namespace TriCodeTest.Controllers
{
    public class OrderInfoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderInfoController(ApplicationDbContext context)
        {
            _context = context;    
        }
        /// <summary>
        /// Retrieves all orders in the DB and returns them as a list.
        /// </summary>
        /// <returns></returns>
        // GET: OrderInfo
        public async Task<IActionResult> Index()
        {
            Order singleOrder = new Order();
            var allOrderInfos = await _context.OrderInfo.Include(oi => oi.User).Where(s => s.Status != Models.Status.Cart && s.Status != Models.Status.Completed).GroupBy(s => s.Status).SelectMany(oi => oi).ToListAsync();
            List<Order> orders = ListOrderDeserialize(allOrderInfos);
            return View(orders);
        }
        /// <summary>
        /// Displays details of order info. Used when it was being loaded in a
        /// separate view.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: OrderInfo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderInfo = await _context.OrderInfo.SingleOrDefaultAsync(m => m.Id == id);
            if (orderInfo == null)
            {
                return NotFound();
            }
            var order = OrderDeserialize(orderInfo);

            return View(order);
        }

        /// <summary>
        /// Returns a single Order by the Id passed in.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: OrderInfo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderInfo = await _context.OrderInfo.SingleOrDefaultAsync(m => m.Id == id);
            if (orderInfo == null)
            {
                return NotFound();
            }
            var order = OrderDeserialize(orderInfo);
            return View(order);
        }

        /// <summary>
        /// Will update the Order in the database with the new status passed in.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        // POST: OrderInfo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateTime,OrderMenuItems,Status,TotalPrice")] Order order)
        {
            OrderInfo orderInfo = OrderSerialize(order);
            if (id != orderInfo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.OrderInfo.Attach(orderInfo);
                    var entry = _context.Entry(orderInfo);
                    entry.Property(s => s.Status).IsModified = true;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderInfoExists(orderInfo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(order);
        }

        /// <summary>
        /// Returns if the Order exists or not.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool OrderInfoExists(int id)
        {
            return _context.OrderInfo.Any(e => e.Id == id);
        }

        /// <summary>
        /// Deserializes an Order and converts it to the view model.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private Order OrderDeserialize(OrderInfo model)
        {
            Order OrderModel = new Order
            {
                Id = model.Id,
                User = model.User,
                DateTime = model.DateTime,
                Status = model.Status,
                TotalPrice = model.TotalPrice,
            };
            List<OrderMenuItem> OrderMenuItems = JsonConvert.DeserializeObject<List<OrderMenuItem>>(model.OrderMenuItems);
            OrderModel.OrderMenuItems = OrderMenuItems;
            return OrderModel;
        }

        /// <summary>
        /// Serializes a view model and converts it to an Order.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private OrderInfo OrderSerialize(Order model)
        {
            OrderInfo OrderInfoModel = new OrderInfo
            {
                Id = model.Id,
                User = model.User,
                DateTime = model.DateTime,
                Status = model.Status,
                TotalPrice = model.TotalPrice,
            };
            string json = JsonConvert.SerializeObject(model.OrderMenuItems);
            OrderInfoModel.OrderMenuItems = json;
            return OrderInfoModel;
        }

        /// <summary>
        /// Deserializes all the Orders in the database to view models.
        /// </summary>
        /// <param name="listOrderInfo"></param>
        /// <returns></returns>
        private List<Order> ListOrderDeserialize(List<OrderInfo> listOrderInfo)
        {
            List<Order> ListOrders = new List<Order>();
            foreach (OrderInfo oi in listOrderInfo)
            {
                Order OrderModel = new Order
                {
                    Id = oi.Id,
                    User = oi.User,
                    DateTime = oi.DateTime,
                    Status = oi.Status,
                    TotalPrice = oi.TotalPrice,
                };
                List<OrderMenuItem> OrderMenuItems = JsonConvert.DeserializeObject<List<OrderMenuItem>>(oi.OrderMenuItems);
                OrderModel.OrderMenuItems = OrderMenuItems;
                ListOrders.Add(OrderModel);
            }
            return ListOrders;
        }
    }
}
