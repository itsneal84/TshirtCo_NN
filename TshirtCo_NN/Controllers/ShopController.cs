using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Stripe;
using TshirtCo_NN.Data;
using TshirtCo_NN.Models;
using TshirtCo_NN.Models.Pages;
using TshirtCo_NN.Models.Repository;

namespace TshirtCo_NN.Controllers
{
    [AllowAnonymous]
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IRepository productRepository;
        private ICategoryRepository categoryRepository;

        public ShopController(ApplicationDbContext context, IRepository prodRepository, ICategoryRepository catRepository)
        {
            _context = context;
            productRepository = prodRepository;
            categoryRepository = catRepository;
        }

        /// <summary>
        /// display all the products from the database on the home page
        /// </summary>
        /// <param name="productOptions"></param>
        /// <param name="catOptions"></param>
        /// <param name="category"></param>
        /// <returns>Home page</returns>
        // GET: Shop
        public async Task<IActionResult> Index([FromQuery(Name = "options")] QueryOptions productOptions, QueryOptions catOptions, Guid category)
        {
            //ViewBag.Categories = categoryRepository.GetCategories(catOptions);
            //ViewBag.SelectedCategory = category;
            //return View(productRepository.GetProduct(productOptions, category));
            var products = from p in _context.Products select p;

            var applicationDbContext = _context.Products.Include(p => p.Categories).Include(p=>p.Colours);
            return View(await applicationDbContext.ToListAsync());
        }

        /// <summary>
        /// display all the products with t-shirts in their name from the database on the home page
        /// </summary>
        /// <returns>T-shirt shop page</returns>
        public async Task<IActionResult> Tshirt()
        {
            var applicationDbContext = _context.Products.Where(p => p.ProductName.Contains("t-shirt"));
            return View(await applicationDbContext.ToListAsync());
        }

        /// <summary>
        /// display all the products with hooded in their name from the database on the home page
        /// </summary>
        /// <returns>Hoodies shop page</returns>
        // GET: Shop/Men
        public async Task<IActionResult> Hoodies()
        {
            var applicationDbContext = _context.Products.Where(p => p.ProductName.Contains("hooded"));
            return View(await applicationDbContext.ToListAsync());
        }

        /// <summary>
        /// display all the products with sweat in their name from the database on the home page
        /// </summary>
        /// <returns>Sweatshirt shop page</returns>
        // GET: Shop/Men
        public async Task<IActionResult> Sweats()
        {
            var applicationDbContext = _context.Products.Where(p => p.ProductName.Contains("sweat"));
            return View(await applicationDbContext.ToListAsync());
        }

        /// <summary>
        /// display all the products where their name starts with men from the database on the home page
        /// </summary>
        /// <returns>Mens shop page</returns>
        // GET: Shop/Men
        public async Task<IActionResult> Men()
        {
            var applicationDbContext = _context.Products.Where(p => p.ProductName.StartsWith("men"));
            return View(await applicationDbContext.ToListAsync());
        }

        /// <summary>
        /// display all the products where their name starts with men & contain t-shirts from the database on the home page
        /// </summary>
        /// <returns>Mens T-shirt shop page</returns>
        // GET: Shop/Men/T-Shirts
        public async Task<IActionResult> MenTshirt()
        {
            var applicationDbContext = _context.Products.Where(p => p.ProductName.Contains("t-shirt") && p.ProductName.StartsWith("men"));
            return View(await applicationDbContext.ToListAsync());
        }

        /// <summary>
        /// display all the products where their name starts with men & contain hooded from the database on the home page
        /// </summary>
        /// <returns>Mens Hoodies shop page</returns>
        // GET: Shop/Men/Hoodies
        public async Task<IActionResult> MenHoodies()
        {
            var applicationDbContext = _context.Products.Where(p => p.ProductName.Contains("hooded") && p.ProductName.StartsWith("men"));
            return View(await applicationDbContext.ToListAsync());
        }

        /// <summary>
        /// display all the products where their name starts with men & contain sweat from the database on the home page
        /// </summary>
        /// <returns>Mens Sweatshirt shop page</returns>
        // GET: Shop/Men/Sweats
        public async Task<IActionResult> MenSweats()
        {
            var applicationDbContext = _context.Products.Where(p => p.ProductName.Contains("sweat") && p.ProductName.StartsWith("men"));
            return View(await applicationDbContext.ToListAsync());
        }

        /// <summary>
        /// display all the products where their name starts with men from the database on the home page
        /// </summary>
        /// <returns>Womens shop page</returns>
        // GET: Shop/Women
        public async Task<IActionResult> Women()
        {
            var applicationDbContext = _context.Products.Where(p => p.ProductName.StartsWith("women"));
            return View(await applicationDbContext.ToListAsync());
        }

        /// <summary>
        /// display all the products where their name starts with women & contain t-shirts from the database on the home page
        /// </summary>
        /// <returns>Womens T-shirt shop page</returns>
        public async Task<IActionResult> WomenTshirt()
        {
            var applicationDbContext = _context.Products.Where(p => p.ProductName.Contains("t-shirt") && p.ProductName.StartsWith("women"));
            return View(await applicationDbContext.ToListAsync());
        }

        /// <summary>
        /// display all the products where their name starts with women & contain hooded from the database on the home page
        /// </summary>
        /// <returns>Womens Hoodies shop page</returns>
        // GET: Shop/Women/Hoodies
        public async Task<IActionResult> WomenHoodies()
        {
            var applicationDbContext = _context.Products.Where(p => p.ProductName.Contains("hooded") && p.ProductName.StartsWith("women"));
            return View(await applicationDbContext.ToListAsync());
        }

        /// <summary>
        /// display all the products where their name starts with women & contain sweat from the database on the home page
        /// </summary>
        /// <returns>Womens Sweatshirt shop page</returns>
        // GET: Shop/Women/Sweatshirts
        public async Task<IActionResult> WomenSweats()
        {
            var applicationDbContext = _context.Products.Where(p => p.ProductName.Contains("sweat") && p.ProductName.StartsWith("women"));
            return View(await applicationDbContext.ToListAsync());
        }

        /// <summary>
        /// displays the product view page for a selected product
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Product view page</returns>
        // GET: Shop/Details/5
        public async Task<IActionResult> ViewProduct(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Categories).Include(p=>p.Colours)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        private bool ProductExists(Guid id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
