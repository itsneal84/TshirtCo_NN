using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TshirtCo_NN.Data;
using TshirtCo_NN.Models;

namespace TshirtCo_NN.Controllers
{
    public class ColoursController : Controller
    {
        /// <summary>
        /// private call to database
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// overloaded contructor for orders controller to set the call to database
        /// </summary>
        /// <param name="context"></param>
        public ColoursController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// action result to populate the index page with list of colours
        /// </summary>
        /// <returns>list of colours</returns>
        // GET: Colours
        public async Task<IActionResult> Index()
        {
            return View(await _context.Colours.ToListAsync());
        }

        /// <summary>
        /// using the passed in colour id gets the selected colour from the database and displays it on the page
        /// </summary>
        /// <param name="id"></param>
        /// <returns>colour details</returns>
        // GET: Colours/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colour = await _context.Colours
                .FirstOrDefaultAsync(m => m.ColourId == id);
            if (colour == null)
            {
                return NotFound();
            }

            return View(colour);
        }

        /// <summary>
        /// action result for creating a colour
        /// </summary>
        /// <returns></returns>
        // GET: Colours/Create
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// overloaded action result for creating a colour
        /// </summary>
        /// <param name="colour"></param>
        /// <returns>new colour</returns>
        // POST: Colours/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ColourId,ColourName")] Colour colour)
        {
            if (ModelState.IsValid)
            {
                colour.ColourId = Guid.NewGuid();
                _context.Add(colour);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(colour);
        }

        /// <summary>
        /// action result for editing a colour
        /// </summary>
        /// <param name="id"></param>
        /// <returns>colour</returns>
        // GET: Colours/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colour = await _context.Colours.FindAsync(id);
            if (colour == null)
            {
                return NotFound();
            }
            return View(colour);
        }

        /// <summary>
        /// overloaded action result for editing a colour
        /// </summary>
        /// <param name="id"></param>
        /// <param name="colour"></param>
        /// <returns>edited colour</returns>
        // POST: Colours/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ColourId,ColourName")] Colour colour)
        {
            if (id != colour.ColourId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(colour);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ColourExists(colour.ColourId))
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
            return View(colour);
        }

        /// <summary>
        /// action result for deleting a colour
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Colours/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colour = await _context.Colours
                .FirstOrDefaultAsync(m => m.ColourId == id);
            if (colour == null)
            {
                return NotFound();
            }

            return View(colour);
        }

        /// <summary>
        /// overloaded action result for deleting a colour
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // POST: Colours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var colour = await _context.Colours.FindAsync(id);
            _context.Colours.Remove(colour);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// checks if colour already exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool ColourExists(Guid id)
        {
            return _context.Colours.Any(e => e.ColourId == id);
        }
    }
}
