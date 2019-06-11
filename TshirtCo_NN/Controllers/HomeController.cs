using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
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
    public class HomeController : Controller
    {
        /// <summary>
        /// private calls to database, usermanager, signinmanager & repository
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private IRepository repository;

        /// <summary>
        /// overloaded contructor for home controller to set the calls to database, usermanager, signinmanager & repository
        /// </summary>
        /// <param name="context"></param>
        /// <param name="repo"></param>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        public HomeController(ApplicationDbContext context, IRepository repo, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            repository = repo;
        }

        /// <summary>
        /// action result to populate the index page with products from the database
        /// </summary>
        /// <returns>list of products</returns>
        // GET: Home:Products
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Products;
            return View(await applicationDbContext.ToListAsync());
        }

        /// <summary>
        /// about page (not used)
        /// </summary>
        /// <returns></returns>
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        /// <summary>
        /// contact page (not used)
        /// </summary>
        /// <returns></returns>
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        /// <summary>
        /// privacy page (not used)
        /// </summary>
        /// <returns></returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// data annotation to limit access to admin, passing in products, orders & customers to the view
        /// </summary>
        /// <returns>admin page</returns>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminArea()
        {
            ViewData["Products"] = _context.Products.Count();
            ViewData["Orders"] = _context.Orders.Count();
            ViewData["Customers"] = _context.Users.Count();

            return View();
        }

        /// <summary>
        /// data annotation to limit access to admin, admin can create a staff member
        /// </summary>
        /// <returns>staff</returns>
        [Authorize(Roles = "Admin")]
        public IActionResult AddStaff()
        {
            return View();
        }

        /// <summary>
        /// displays a list of users on users page
        /// </summary>
        /// <returns>users</returns>
        public IActionResult Users()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
