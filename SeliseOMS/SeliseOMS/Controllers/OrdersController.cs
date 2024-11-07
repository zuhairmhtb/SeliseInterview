using MessageBorker.Configuration;
using MessageBroker.Configuration;
using MessageBroker.Events;
using MessageBroker.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeliseOMS.Data;
using SeliseOMS.Models.Orders;
using SeliseOMS.ViewModel;

namespace SeliseOMS.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRabbitMQPublisher<PlaceOrderEvent> _orderPublisher;
        private readonly IRabbitMQPublisher<UpdateOrderstatusEvent> _orderStatusPublisher;

        public OrdersController(
            ApplicationDbContext context,
            IRabbitMQPublisher<PlaceOrderEvent> orderPublisher,
            IRabbitMQPublisher<UpdateOrderstatusEvent> orderStatusPublisher
            )
        {
            _context = context;
            _orderPublisher = orderPublisher;
            _orderStatusPublisher = orderStatusPublisher;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            return _context.Orders != null ?
                        View(await _context.Orders.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Orders'  is null.");
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Status,CreatedAt,Total")] Order order)
        {
            if (ModelState.IsValid)
            {
                var mqEvent = new MQEvent<PlaceOrderEvent>()
                {
                    EventName = RabbitMQEvents.PlaceOrder,
                    Message = new PlaceOrderEvent()
                    {
                        OrderId = order.Id,
                        Total = order.Total,
                        Products = new List<int>() { 1, 2 } // Fix it after integrating product functionality

                    }
                };
                await _orderPublisher.PublishMessageAsync(mqEvent, RabbitMQQueues.OrderQueue);

                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Status,CreatedAt,Total")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Orders'  is null.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateStatus([FromQuery] int orderId)
        {
            return View(new UpdateStatusViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(UpdateStatusViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            var mqEvent = new MQEvent<UpdateOrderstatusEvent>()
            {
                EventName = RabbitMQEvents.UpdateOrderStatus,
                Message = new UpdateOrderstatusEvent()
                {
                    OrderId = model.OrderId,
                    OrderStatus = model.OrderStatus
                }
            };
            await _orderStatusPublisher.PublishMessageAsync(mqEvent, RabbitMQQueues.OrderQueue);
            return RedirectToAction("Index");
        }

    }
}
