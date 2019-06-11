using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TshirtCo_NN.Data;
using TshirtCo_NN.Models;

namespace TshirtCo_NN.Controllers
{
    public class DesignsController : Controller
    {
        /// <summary>
        /// private calls to database & hosting environment
        /// </summary>
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _environment;

        /// <summary>
        /// overloaded contructor for designs controller to set the calls to database & hosting environment
        /// </summary>
        /// <param name="context"></param>
        /// <param name="environment"></param>
        public DesignsController(ApplicationDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        /// <summary>
        /// action result to populate the index page with a list of designs
        /// </summary>
        /// <returns>index page</returns>
        // GET: Designs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Designs.ToListAsync());
        }

        /// <summary>
        /// action result to get details for a selected design
        /// </summary>
        /// <param name="id"></param>
        /// <returns>design details</returns>
        // GET: Designs/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var design = await _context.Designs
                .FirstOrDefaultAsync(m => m.DesignId == id);
            if (design == null)
            {
                return NotFound();
            }

            return View(design);
        }

        /// <summary>
        /// action result to create new design
        /// </summary>
        /// <returns>design</returns>
        // GET: Designs/Create
        public IActionResult Create()
        {
            ViewData["ColourId"] = new SelectList(_context.Colours, "ColourId", "ColourName");
            return View();
        }

        /// <summary>
        /// overloaded action result to create new design
        /// </summary>
        /// <param name="design"></param>
        /// <returns>new design</returns>
        // POST: Designs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DesignId,DesignName,GarmentColourForPrint")] Design design)
        {
            if (!ModelState.IsValid)
            {
                return View(design);
            }

            design.DesignId = Guid.NewGuid();
            _context.Add(design);
            await _context.SaveChangesAsync();
            ViewData["ColourId"] = new SelectList(_context.Colours, "ColourId", "ColourName", design.DesignId);

            //save image
            string webRootPath = _environment.WebRootPath;
            //store uploaded image from view
            var file = HttpContext.Request.Form.Files;
            //connect to database & find designId
            var dbDesign = _context.Designs.Find(design.DesignId);
            //check for previously added files
            if (file.Count != 0)
            {
                //if image has been uploaded find previous path & extension
                var upload = Path.Combine(webRootPath, Image.DesignImageFolder);
                var extension = Path.GetExtension(file[0].FileName);
                //copy uploaded file using filestream to server with updated name
                using (var fileStream =
                    new FileStream(Path.Combine(upload, design.DesignId + extension), FileMode.Create))
                {
                    file[0].CopyTo(fileStream);
                }

                dbDesign.Image = @"\" + Image.DesignImageFolder + @"\" + design.DesignId + extension;
            }
            else
            {
                var upload = Path.Combine(webRootPath, Image.DesignImageFolder + @"\" + Image.DefaultDesignImage);
                System.IO.File.Copy(upload, webRootPath + @"\" + Image.DesignImageFolder + @"\" + design.DesignId + ".jpg");
                dbDesign.Image = @"\" + Image.DesignImageFolder + @"\" + design.DesignId + ".jpg";
            }
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// action result to edit selected design
        /// </summary>
        /// <param name="id"></param>
        /// <returns>design</returns>
        // GET: Designs/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var design = await _context.Designs.FindAsync(id);
            if (design == null)
            {
                return NotFound();
            }
            return View(design);
        }

        /// <summary>
        /// overloaded action result to edit selected design
        /// </summary>
        /// <param name="id"></param>
        /// <param name="design"></param>
        /// <returns>design</returns>
        // POST: Designs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("DesignId,DesignName,GarmentColourForPrint")] Design design)
        {
            if (id != design.DesignId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(design);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DesignExists(design.DesignId))
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
            return View(design);
        }

        /// <summary>
        /// action result to delete a design
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Designs/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var design = await _context.Designs
                .FirstOrDefaultAsync(m => m.DesignId == id);
            if (design == null)
            {
                return NotFound();
            }

            return View(design);
        }

        /// <summary>
        /// action result to confirm delete a design
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // POST: Designs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var design = await _context.Designs.FindAsync(id);
            _context.Designs.Remove(design);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// checks if a design already exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool DesignExists(Guid id)
        {
            return _context.Designs.Any(e => e.DesignId == id);
        }
    }
}
