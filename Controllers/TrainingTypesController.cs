using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> Search(string searchString)
        {
            var Ttype = from a in _context.TrainingTypes
                        select a;
            if (!String.IsNullOrEmpty(searchString))
            {
                Ttype = Ttype.Where(s => s.Name.Contains(searchString)|| s.Target.Contains(searchString));
            }
            var ids = Ttype.Select(s => s.TrainingTypeId);
            List<TrainingType> typesToShow = await _context.TrainingTypes.Where(x => ids.Contains(x.TrainingTypeId)).ToListAsync();

            return View("~/Views/TrainingTypes/index.cshtml", typesToShow);
        }

        // GET: TrainingTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View("~/Views/Home/Index.cshtml");
            }

            var trainingType = await _context.TrainingTypes
                .FirstOrDefaultAsync(m => m.TrainingTypeId == id);
            if (trainingType == null)
            {
                return View("~/Views/Home/Index.cshtml");
            }

            return View(trainingType);
        }

        // GET: TrainingTypes/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: TrainingTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TrainingTypeId,Name,Target")] TrainingType trainingType)
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("~/Views/Home/Index.cshtml");
            }

            var trainingType = await _context.TrainingTypes.FindAsync(id);
            if (trainingType == null)
            {
                return View("~/Views/Home/Index.cshtml");
            }
            return View(trainingType);
        }

        // POST: TrainingTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TrainingTypeId,Name,Target")] TrainingType trainingType)
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
                        return View("~/Views/Home/Index.cshtml");
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

        [Authorize(Roles = "Admin")]
        // GET: TrainingTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("~/Views/Home/Index.cshtml");
            }

            var trainingType = await _context.TrainingTypes
                .FirstOrDefaultAsync(m => m.TrainingTypeId == id);
            if (trainingType == null)
            {
                return View("~/Views/Home/Index.cshtml");
            }

            return View(trainingType);
        }

        // POST: TrainingTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
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
