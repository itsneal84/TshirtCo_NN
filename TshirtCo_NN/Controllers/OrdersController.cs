using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TshirtCo_NN.Data;
using TshirtCo_NN.Models;
using TshirtCo_NN.Models.Repository;

namespace TshirtCo_NN.Controllers
{
    /// <summary>
    /// data annotation to allow anyone to access views
    /// </summary>
    [AllowAnonymous]
    public class OrdersController : Controller
    {
        /// <summary>
        /// private calls to database, product repository & order repository
        /// </summary>
        private readonly ApplicationDbContext _context;
        private IRepository productRepository;
        private IOrdersRepository orderRepository;

        /// <summary>
        /// overloaded contructor for orders controller to set the calls to database, product repository & order repository
        /// </summary>
        /// <param name="context"></param>
        /// <param name="prodRepository"></param>
        /// <param name="ordRepository"></param>
        public OrdersController(ApplicationDbContext context, IRepository prodRepository, IOrdersRepository ordRepository)
        {
            _context = context;
            productRepository = prodRepository;
            orderRepository = ordRepository;
        }

        /// <summary>
        /// action result to populate the index page with orders from the orders repository
        /// </summary>
        /// <returns>orders home page</returns>
        // GET: Orders
        public IActionResult Index() => View(orderRepository.Orders);

        /// <summary>
        /// using the passed in order id gets the selected order from the order repository and diaplyes it on the page
        /// authorization set to admin only so no other user can access it
        /// </summary>
        /// <param name="id"></param>
        /// <returns>an order</returns>
        [Authorize(Roles = "Admin")]
        // GET: Orders/Edit/5
        public IActionResult Edit(Guid id)
        {
            var products = productRepository.Products;
            Order order = id == null ? new Order() : orderRepository.GetOrder(id);
            IDictionary<Guid, OrderLine> lineMap = order.OrderLines.ToDictionary(d => d.ProductId) ?? new Dictionary<Guid, OrderLine>();
            ViewBag.OrderLines = products.Select(p=>lineMap.ContainsKey(p.ProductId)?lineMap[p.ProductId]:new OrderLine { Product = p, ProductId = p.ProductId, Quantity = 0 });
            return View(order);
        }

        /// <summary>
        /// using the passed in order id gets the selected order from the order repository and displays it on the page
        /// authorization set to admin only so no other user can access it
        /// </summary>
        /// <param name="id"></param>
        /// <returns>an order</returns>
        [Authorize(Roles = "Admin")]
        // GET: Orders/Edit/5
        public IActionResult Details(Guid id)
        {
            var products = productRepository.Products;
            Order order = id == null ? new Order() : orderRepository.GetOrder(id);
            IDictionary<Guid, OrderLine> lineMap = order.OrderLines.ToDictionary(d => d.ProductId) ?? new Dictionary<Guid, OrderLine>();
            ViewBag.OrderLines = products.Select(p => lineMap.ContainsKey(p.ProductId) ? lineMap[p.ProductId] : new OrderLine { Product = p, ProductId = p.ProductId, Quantity = 0 });
            return View(order);
        }

        /// <summary>
        /// get details for orders that belong to the current user
        /// </summary>
        /// <param name="id"></param>
        /// <returns>an order</returns>
        // GET: Orders/MyOrderDetails/5
        public IActionResult MyOrderDetails(Guid id)
        {
            var products = productRepository.Products;
            Order order = id == null ? new Order() : orderRepository.GetOrder(id);
            IDictionary<Guid, OrderLine> lineMap = order.OrderLines.ToDictionary(d => d.ProductId) ?? new Dictionary<Guid, OrderLine>();
            ViewBag.OrderLines = products.Select(p => lineMap.ContainsKey(p.ProductId) ? lineMap[p.ProductId] : new OrderLine { Product = p, ProductId = p.ProductId, Quantity = 0 });
            return View(order);
        }

        /// <summary>
        /// passes in the new order details & checks it against the original and calls the update order method from the orders repository
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddOrUpdateOrder(Order order)
        {
            order.OrderLines = order.OrderLines.Where(l => l.OrderLineId != null || (l.OrderLineId == null && l.Quantity > 0)).ToArray();
            if (order.OrderId == null)
            {
                orderRepository.AddOrder(order);
            }
            else
            {
                orderRepository.UpdateOrder(order);
            }
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// passes in the order id from the view, checks id's match and calls the delete order method from orders repository
        /// </summary>
        /// <param name="id"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        /// <summary>
        /// passes in the order id from the view, checks id's match and calls the delete order method from orders repository
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var order = await _context.Orders.FindAsync(id);
            orderRepository.DeleteOrder(order);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// get orders that belong to the current user
        /// </summary>
        /// <returns>user orders</returns>
        // Get: UserOrders
        public IActionResult UserOrders(Guid id)
        {
            var order = orderRepository.Orders.Where(o => o.Email == User.Identity.Name);
            return View(order);
        }

        /// <summary>
        /// when order is marked as dispatched update product quantites in database to match removed stock
        /// </summary>
        /// <param name="ProductId"></param>
        /// <param name="product"></param>
        /// <param name="orderLine"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Dispatched(Guid ProductId, OrderLine orderLine, Order order)
        {
            //if (product.ProductId != ProductId)
            //{
            //    return NotFound();
            //}

            try
            {
                var newStock = _context.Products.Where(p => p.ProductId == ProductId);
                //var newStock = _context.Products.Single(p => p.ProductId == product.ProductId);
                foreach (var prod in newStock)
                {
                    prod.Small = prod.Small - orderLine.Small;
                    prod.Medium = prod.Medium - orderLine.Medium;
                    prod.Large = prod.Large - orderLine.Large;
                    prod.XLarge = prod.XLarge - orderLine.XLarge;
                }


                var orderLineDispatched = _context.OrderLines.Single(o => o.OrderLineId == orderLine.OrderId);
                orderLineDispatched.Dispatched = true;
                orderLineDispatched.DateShipped = DateTime.Today;
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                 throw;
            }
            return RedirectToAction("Index");
        }
    }
}
