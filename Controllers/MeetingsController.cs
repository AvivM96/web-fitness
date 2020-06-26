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
    public class MeetingsController : Controller
    {
        private readonly fitnessdataContext _context;

        public MeetingsController(fitnessdataContext context)
        {
            _context = context;
        }

        // GET: Meetings
        public async Task<IActionResult> Index()
        {
            var fitnessdataContext = _context.Meetings.Include(m => m.TrainType).Include(m => m.Trainer);
            return View(await fitnessdataContext.ToListAsync());
        }

        // GET: Meetings/Details/5
        public async Task<IActionResult> Details(DateTime? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meeting = await _context.Meetings
                .Include(m => m.TrainType)
                .Include(m => m.Trainer)
                .FirstOrDefaultAsync(m => m.MeetDate == id);
            if (meeting == null)
            {
                return NotFound();
            }

            return View(meeting);
        }

        // GET: Meetings/Create
        public IActionResult Create()
        {
            ViewData["TrainingTypeID"] = new SelectList(_context.TrainingTypes, "TrainingTypeId", "Name");
            ViewData["TrainerID"] = new SelectList(_context.Trainers, "TrainerId", "Address");
            return View();
        }

        // POST: Meetings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TrainingTypeID,TrainerID,MeetDate,MeetNum")] Meeting meeting)
        {
            if (ModelState.IsValid)
            {
                _context.Add(meeting);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TrainingTypeID"] = new SelectList(_context.TrainingTypes, "TrainingTypeId", "Name", meeting.TrainingTypeID);
            ViewData["TrainerID"] = new SelectList(_context.Trainers, "TrainerIDe", "TraineriD", meeting.TrainerID);
            return View(meeting);
        }

        // GET: Meetings/Edit/5
        public async Task<IActionResult> Edit(DateTime? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meeting = await _context.Meetings.FindAsync(id);
            if (meeting == null)
            {
                return NotFound();
            }
            ViewData["TrainingTypeID"] = new SelectList(_context.TrainingTypes, "TrainingTypeId", "Name", meeting.TrainingTypeID);
            ViewData["TrainerID"] = new SelectList(_context.Trainers, "TrainerId", "Address", meeting.TrainerID);
            return View(meeting);
        }

        // POST: Meetings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DateTime id, [Bind("TrainingTypeID,TrainerID,MeetDate,MeetNum")] Meeting meeting)
        {
            if (id != meeting.MeetDate)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(meeting);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MeetingExists(meeting.MeetDate))
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
            ViewData["TrainingTypeID"] = new SelectList(_context.TrainingTypes, "TrainingTypeId", "Name", meeting.TrainingTypeID);
            ViewData["TrainerID"] = new SelectList(_context.Trainers, "TrainerId", "Address", meeting.TrainerID);
            return View(meeting);
        }

        // GET: Meetings/Delete/5
        public async Task<IActionResult> Delete(DateTime? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meeting = await _context.Meetings
                .Include(m => m.TrainType)
                .Include(m => m.Trainer)
                .FirstOrDefaultAsync(m => m.MeetDate == id);
            if (meeting == null)
            {
                return NotFound();
            }

            return View(meeting);
        }
        public async Task<IActionResult> List(string eventTypeName)
        {
            var meetings_of_type = await _context.Meetings.Where(m => m.TrainType.Name == eventTypeName).ToListAsync();
            return View(meetings_of_type);
        }

        // POST: Meetings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(DateTime id)
        {
            var meeting = await _context.Meetings.FindAsync(id);
            _context.Meetings.Remove(meeting);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MeetingExists(DateTime id)
        {
            return _context.Meetings.Any(e => e.MeetDate == id);
        }
    }
}
