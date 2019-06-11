using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using TshirtCo_NN.Data;
using TshirtCo_NN.Models;

namespace TshirtCo_NN.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _environment;

        public ProductsController(ApplicationDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Products.Include(p => p.Categories).Include(p=>p.Colours);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Products/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid? id)
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

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewData["ColourId"] = new SelectList(_context.Colours, "ColourId", "ColourName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,Price,StockLvl,ColourId,Small,Medium,Large,XLarge,Image,CategoryId")] Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            product.ProductId = Guid.NewGuid();
            _context.Add(product);
            await _context.SaveChangesAsync();
            
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["ColourId"] = new SelectList(_context.Colours, "ColourId", "ColourName", product.Colours);

            //save image
            string webRootPath = _environment.WebRootPath;
            //store uploaded image from view
            var file = HttpContext.Request.Form.Files;
            //connect to database & find productId
            var dbProduct = _context.Products.Find(product.ProductId);
            //check for previously uploaded files
            if (file.Count != 0)
            {
                //if image has been uploaded find previous path & extension
                var upload = Path.Combine(webRootPath, Image.ProductImageFolder);
                var extension = Path.GetExtension(file[0].FileName);
                //copy uploaded file using filestream to server with updated name 
                using (var fileStream =
                    new FileStream(Path.Combine(upload, product.ProductId + extension), FileMode.Create))
                {
                    file[0].CopyTo(fileStream);
                }

                dbProduct.Image = @"\" + Image.ProductImageFolder + @"\" + product.ProductId + extension;
            }
            else
            {
                var upload = Path.Combine(webRootPath, Image.ProductImageFolder + @"\" + Image.DefaultProductImage);
                System.IO.File.Copy(upload, webRootPath + @"\" + Image.ProductImageFolder + @"\" + product.ProductId + ".jpg");
                dbProduct.Image = @"\" + Image.ProductImageFolder + @"\" + product.ProductId + ".jpg";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["ColourId"] = new SelectList(_context.Colours, "ColourId", "ColourName", product.Colours);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ProductId,ProductName,Price,StockLvl,ColourId,Small,Medium,Large,XLarge,Image,CategoryId")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["ColourId"] = new SelectList(_context.Colours, "ColourId", "ColourName", product.Colours);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
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

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(Guid id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
