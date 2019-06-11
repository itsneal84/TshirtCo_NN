using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Stripe;
using TshirtCo_NN.Data;
using TshirtCo_NN.Models;
using TshirtCo_NN.Models.Repository;
using Order = TshirtCo_NN.Models.Order;
using Product = TshirtCo_NN.Models.Product;

namespace TshirtCo_NN.Controllers
{
    [ViewComponent(Name="Cart")]
    public class CartController : Controller
    {
        /// <summary>
        /// private calls to database, product repository & order repository
        /// </summary>
        private readonly ApplicationDbContext _context;
        private IRepository productRepository;
        private IOrdersRepository ordersRepository;

        /// <summary>
        /// overloaded contructor for orders controller to set the calls to database, product repository & order repository
        /// </summary>
        /// <param name="prorep"></param>
        /// <param name="ordrep"></param>
        public CartController(IRepository prorep, IOrdersRepository ordrep)
        {
            productRepository = prorep;
            ordersRepository = ordrep;
        }

        public IViewComponentResult Invoke(ISession session)
        {
            return new ViewViewComponentResult
            {
                ViewData = new ViewDataDictionary<Cart>(ViewData,session.GetJson<Cart>("Cart"))
            };
        }

        [AllowAnonymous]
        public IActionResult Index(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View(GetCart());
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult AddToCart(Product product, string returnUrl)
        {
            SaveCart(GetCart().AddItem(product));
            return RedirectToAction(nameof(Index), new {returnUrl});
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult RemoveFromCart(Guid productId, string returnUrl)
        {
            SaveCart(GetCart().RemoveItem(productId));
            return RedirectToAction(nameof(Index), new { returnUrl });
        }

        public IActionResult CreateOrder()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateOrder(Order order, string stripeToken)
        {
            order.OrderLines = GetCart().OlList.Select(s => new OrderLine
            {
                ProductId = s.ProductId,
                Name = s.Name,
                Colour = s.Colour,
                Small = s.Small,
                Medium = s.Medium,
                Large = s.Large,
                XLarge = s.XLarge,
                Quantity = s.Quantity,
                Price = s.Price,
                OrderLineTotal = s.Quantity * s.Price,
            }).ToArray();
            order.OrderTotal = order.OrderLines.Sum(t => t.Quantity * t.Price);
            order.OrderDate = DateTime.Now;
            //ordersRepository.AddOrder(order);
            //SaveCart(new Cart());

            // Set your secret key: remember to change this to your live secret key in production
            // See your keys here: https://dashboard.stripe.com/account/apikeys
            StripeConfiguration.SetApiKey("sk_test_XpUEuxlBSf7e0gQ4no3ljNio00JKU45AHe");

            // Token is created using Checkout or Elements!
            // Get the payment token submitted by the form:
            var token = stripeToken; // Using ASP.NET MVC

            var options = new ChargeCreateOptions
            {
                Amount = Convert.ToInt32(order.OrderTotal * 100),
                Currency = "gbp",
                Description = "Payment for order from The T-shirt Co",
                SourceId = token,
                ReceiptEmail = order.Email,
            };
            var service = new ChargeService();
            Charge charge = service.Create(options);

            var model = new ChargeViewModel();
            model.ChargeId = charge.Id;
            order.PaymentRef = model.ChargeId;

            ordersRepository.AddOrder(order);
            SaveCart(new Cart());

            return View("OrderStatus", model);
            //return View("OrderStatus");
        }

        //public IActionResult Completed() => View();

        //// POST: Cart/Payment
        //[Authorize]
        //[HttpPost]
        //public IActionResult CreatePayment(string stripeToken)
        //{
        //    return View();
        //}

        //// GET: Cart/Payment
        //[Authorize]
        //[HttpPost]
        //public IActionResult CreatePayment()
        //{
        //    return View();
        //}

        private Cart GetCart() => HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
        private void SaveCart(Cart cart) => HttpContext.Session.SetJson("Cart", cart);

        private void GetTotal(OrderLine orderLine)
        {
            var total = orderLine.OrderLineTotal;
        }
    }
}