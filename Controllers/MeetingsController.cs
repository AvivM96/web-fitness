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
        public async Task<IActionResult> Index(int? training_type)
        {
            List<Meeting> meetings;
            if (training_type != null)
            {
                meetings = await _context.Meetings
                    .Include(m => m.TrainType)
                    .Include(m => m.Trainer)
                    .Where(m => m.TrainingTypeID == training_type).ToListAsync();
            }
            else
            {
                meetings = await _context.Meetings
                    .Include(m => m.TrainType)
                    .Include(m => m.Trainer).ToListAsync();
            }
            return View(meetings);
        }

        // GET: Meetings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meeting = await _context.Meetings
                .Include(m => m.TrainType)
                .Include(m => m.Trainer)
                .FirstOrDefaultAsync(m => m.MeetID == id);
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
            ViewData["TrainerID"] = new SelectList(_context.Trainers, "TrainerId", "Mail");
            return View();
        }

        // POST: Meetings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MeetID,TrainingTypeID,TrainerID,MeetDate")] Meeting meeting)
        {
            if (ModelState.IsValid)
            {
                _context.Add(meeting);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TrainingTypeID"] = new SelectList(_context.TrainingTypes, "TrainingTypeId", "Name");
            ViewData["TrainerID"] = new SelectList(_context.Trainers, "TrainerId", "Mail");
            return View(meeting);
        }

        // GET: Meetings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meeting = await _context.Meetings
                .Include(m => m.TrainType)
                .Include(m => m.Trainer)
                .FirstOrDefaultAsync(m => m.MeetID == id);

             if (meeting == null)
            {
                return NotFound();
            }
            ViewData["TrainingTypeID"] = new SelectList(_context.TrainingTypes, "TrainingTypeId", "Name");
            ViewData["TrainerID"] = new SelectList(_context.Trainers, "TrainerId", "Mail");
            return View(meeting);
        }

        // POST: Meetings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MeetID,TrainingTypeID,TrainerID,MeetDate")] Meeting meeting)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(meeting);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MeetingExists(meeting.MeetID))
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
            ViewData["TrainerID"] = new SelectList(_context.Trainers, "TrainerId", "Mail", meeting.TrainerID);
            return View(meeting);
        }

        // GET: Meetings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meeting = await _context.Meetings
                .Include(m => m.TrainType)
                .Include(m => m.Trainer)
                .FirstOrDefaultAsync(m => m.MeetID == id);
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var meeting = await _context.Meetings.FindAsync(id);
            _context.Meetings.Remove(meeting);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MeetingExists(int id)
        {
            return _context.Meetings.Any(e => e.MeetID == id);
        }
    }
}
