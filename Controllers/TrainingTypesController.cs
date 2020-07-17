using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web_fitness.Data;
using web_fitness.Models;

namespace web_fitness.Controllers
{
    public class TrainingTypesController : Controller
    {
        private readonly fitnessdataContext _context;

        public TrainingTypesController(fitnessdataContext context)
        {
            _context = context;
        }

        // GET: TrainingTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.TrainingTypes.ToListAsync());
        }

        // GET: TrainingTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingType = await _context.TrainingTypes
                .FirstOrDefaultAsync(m => m.TrainingTypeId == id);
            if (trainingType == null)
            {
                return NotFound();
            }

            return View(trainingType);
        }

        // GET: TrainingTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TrainingTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Target")] TrainingType trainingType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trainingType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(trainingType);
        }

        // GET: TrainingTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingType = await _context.TrainingTypes.FindAsync(id);
            if (trainingType == null)
            {
                return NotFound();
            }
            return View(trainingType);
        }

        // POST: TrainingTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Target")] TrainingType trainingType)
        { 
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainingType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainingTypeExists(trainingType.TrainingTypeId))
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
            return View(trainingType);
        }

        // GET: TrainingTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingType = await _context.TrainingTypes
                .FirstOrDefaultAsync(m => m.TrainingTypeId == id);
            if (trainingType == null)
            {
                return NotFound();
            }

            return View(trainingType);
        }

        // POST: TrainingTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainingType = await _context.TrainingTypes.FindAsync(id);
            _context.TrainingTypes.Remove(trainingType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainingTypeExists(int id)
        {
            return _context.TrainingTypes.Any(e => e.TrainingTypeId == id);
        }
    }
}
