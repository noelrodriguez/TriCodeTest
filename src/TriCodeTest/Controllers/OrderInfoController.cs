using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TriCodeTest.Data;
using TriCodeTest.Models.OrderModels;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using TriCodeTest.TwilioNotification;

namespace TriCodeTest.Controllers
{
    /// <summary>
    /// Order info class
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Authorize(Roles = "Admin, Staff")]
    public class OrderInfoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        /// <summary>
        /// Order info constructor
        /// </summary>
        /// <param name="context">context</param>
        /// <param name="loggerFactory">loggerFactory</param>
        public OrderInfoController(ApplicationDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<OrderInfoController>();
        }
        /// <summary>
        /// Retrieves all orders in the DB and returns them as a list.
        /// </summary>
        /// <returns>
        /// View containing list of orders
        /// </returns>
        // GET: OrderInfo
        public async Task<IActionResult> Index()
        {
            Order singleOrder = new Order();
            var allOrderInfos = await _context.OrderInfo.Include(oi => oi.User).Where(s => s.Status != Models.Status.Cart && s.Status != Models.Status.Completed).GroupBy(s => s.Status).SelectMany(oi => oi).ToListAsync();
            List<Order> orders = ListOrderDeserialize(allOrderInfos);
            return View(orders);
        }

        /// <summary>
        /// Returns a single Order by the Id passed in.
        /// </summary>
        /// <param name="id">Id of an order</param>
        /// <returns>
        /// View to edit single order
        /// </returns>
        // GET: OrderInfo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var orderInfo = await _context.OrderInfo.Include(usr => usr.User).SingleOrDefaultAsync(m => m.Id == id);
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
        /// <param name="id">Id of an order</param>
        /// <param name="order">Order object passed in by form</param>
        /// <returns>
        /// Redirect to Index view upon success or editable view of same order object on failure
        /// </returns>
        // POST: OrderInfo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateTime,OrderMenuItems,Status,TotalPrice,UserId")] Order order)
        {
            OrderInfo orderInfo = OrderSerialize(order);
            var numberToSms = _context.Users.SingleOrDefault(usr => usr.Id == order.UserId).PhoneNumber;
            bool success;
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

                    if (order.Status.Equals(Models.Status.Pick_Up))
                    {
                        try
                        {
                            success = Notification.SendNotification(numberToSms, "Your order is ready for pickup. DO NOT REPLY! Data rates may apply");
                            ViewBag.Messag = "PhoneNumber incorrect ";
                            if (success)
                            {
                                //ViewBag.Messag = "Message sent";
                                return RedirectToAction("Index");
                            }
                        }
                        catch (Exception e)
                        {
                            _logger.LogWarning(e.Message);
                            return View(order);
                        }
                    }
                    return RedirectToAction("Index");
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
            }
            return View(order);
        }

        /// <summary>
        /// Returns if the Order exists or not.
        /// </summary>
        /// <param name="id">Id of an order</param>
        /// <returns>
        /// order item
        /// </returns>
        private bool OrderInfoExists(int id)
        {
            return _context.OrderInfo.Any(e => e.Id == id);
        }

        /// <summary>
        /// Deserializes an Order and converts it to the view model.
        /// </summary>
        /// <param name="model">Order model from the database</param>
        /// <returns>
        /// View model of order with deserialized JSON for items in order
        /// </returns>
        private Order OrderDeserialize(OrderInfo model)
        {
            Order OrderModel = new Order
            {
                Id = model.Id,
                User = model.User,
                DateTime = model.DateTime,
                Status = model.Status,
                TotalPrice = model.TotalPrice,
                UserId = model.User.Id
            };
            List<OrderMenuItem> OrderMenuItems = JsonConvert.DeserializeObject<List<OrderMenuItem>>(model.OrderMenuItems);
            OrderModel.OrderMenuItems = OrderMenuItems;
            return OrderModel;
        }

        /// <summary>
        /// Serializes a view model and converts it to an Order.
        /// </summary>
        /// <param name="model">Order view model</param>
        /// <returns>
        /// Table model from the database with serialized JSON
        /// </returns>
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
        /// <param name="listOrderInfo">List of order models from the database</param>
        /// <returns>
        /// List of view models of orders with deserialized JSON for items in orders
        /// </returns>
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
