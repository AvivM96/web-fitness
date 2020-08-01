using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web_fitness.Data;
using web_fitness.Models;

namespace web_fitness.Controllers
{
    public class TrainersController : Controller
    {
        private readonly fitnessdataContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TrainersController(fitnessdataContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Trainers
        public async Task<IActionResult> Index()
        {
            List<ApplicationUser> trainers = await _context.AspNetUsers.Where(user => user.IsTrainer).ToListAsync();
            return View(trainers);
        }
        public async Task<IActionResult> Search(string searchString)
        {
            var users = from a in _context.AspNetUsers.Where(user => user.IsTrainer)
                        select a;
            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.City.Contains(searchString) || s.Gender.Contains(searchString) || s.FirstName.Contains(searchString) || s.LastName.Contains(searchString) || s.Email.Contains(searchString) || s.Address.Contains(searchString) || s.PhoneNumber.Contains(searchString));
            }
            var ids = users.Select(s => s.Id);
            List<ApplicationUser> usersToShow = await _context.AspNetUsers.Where(x => ids.Contains(x.Id)).ToListAsync();

            return View("~/Views/Trainers/index.cshtml", usersToShow);
        }



        // GET: Trainers/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainer = await _context.AspNetUsers
                .FirstOrDefaultAsync(m => m.Id == id && m.IsTrainer);

            if (trainer == null)
            {
                return NotFound();
            }

            return View(trainer);
        }

        // GET: Trainers/Create
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TrainerId,Mail,TrainerName,TrainerPhone,TrainerGender,Address,City")] ApplicationUser trainer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    return View(trainer);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        // GET: Trainers/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainer = await _context.AspNetUsers.FindAsync(id);
            if (trainer == null)
            {
                return NotFound();
            }
            return View(trainer);
        }

        // POST: Trainers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Email,FirstName,LastName,PhoneNumber,Gender,Address,City,IsTrainer")] ApplicationUser trainer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainerExists(trainer.Id))
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
            return View(trainer);
        }

        // GET: Trainers/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainer = await _context.AspNetUsers
                .FirstOrDefaultAsync(m => m.Id == id && m.IsTrainer);
            if (trainer == null)
            {
                return NotFound();
            }

            return View(trainer);
        }

        // POST: Trainers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var trainer = await _context.AspNetUsers.FindAsync(id);
            _context.AspNetUsers.Remove(trainer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Meetings
        [Authorize(Roles = "Trainer")]
        public async Task<IActionResult> Meetings()
        {
            var trainerId = _userManager.GetUserId(User);
            List<Meeting> meetings = await _context.Meetings
                    .Include(m => m.TrainType)
                    .Include(m => m.Trainer)
                    .Where(m => m.TrainerID == trainerId)
                    .ToListAsync();
            return View(meetings);
        }

        private bool TrainerExists(string id)
        {
            return _context.AspNetUsers.Any(e => e.Id == id && e.IsTrainer);
        }
    }
}
