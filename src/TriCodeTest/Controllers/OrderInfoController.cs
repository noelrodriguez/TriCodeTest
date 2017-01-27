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

        // GET: OrderInfo
        public async Task<IActionResult> Index()
        {
            Order singleOrder = new Order();
            var allOrderInfos = await _context.OrderInfo.Include(oi => oi.User).Where(s => s.Status != Models.Status.Cart && s.Status != Models.Status.Completed).GroupBy(s => s.Status).SelectMany(oi => oi).ToListAsync();
            List<Order> orders = ListOrderDeserialize(allOrderInfos);
            return View(orders);
        }

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

        /*
        // GET: OrderInfo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OrderInfo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateTime,OrderMenuItems,Status,TotalPrice")] OrderInfo orderInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(orderInfo);
        }*/

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

        /*
        // GET: OrderInfo/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

            return View(orderInfo);
        }

        // POST: OrderInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderInfo = await _context.OrderInfo.SingleOrDefaultAsync(m => m.Id == id);
            _context.OrderInfo.Remove(orderInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        */

        private bool OrderInfoExists(int id)
        {
            return _context.OrderInfo.Any(e => e.Id == id);
        }

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
