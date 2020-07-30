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
            TrainbyCityGraph();
            CountMeetingbyTypeGraph();
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
            ViewData["TrainerID"] = new SelectList(_context.AspNetUsers.Where(t => t.IsTrainer).ToList(), "Id", "Email");
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
            ViewData["TrainerID"] = new SelectList(_context.AspNetUsers.Where(t => t.IsTrainer).ToList(), "Id", "Email");
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
            ViewData["TrainerID"] = new SelectList(_context.AspNetUsers.Where(t => t.IsTrainer).ToList(), "Id", "Email");
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
            ViewData["TrainerID"] = new SelectList(_context.AspNetUsers.Where(t => t.IsTrainer).ToList(), "Id", "Email", meeting.TrainerID);
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
            var meetings_of_type = await _context.Meetings.Include(m => m.Trainer).Include(m => m.TrainType).Where(m => m.TrainType.Name == eventTypeName).ToListAsync();
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


        public void TrainbyCityGraph() //create data for the first graph
        { //calculate the train meetings  per city
            var trainPerCity = from s in _context.Meetings
                               join a in _context.AspNetUsers
                               on s.TrainerID equals a.Id
                               group a by a.City into city_count
                               select new
                               {
                                   key = city_count.Key,
                                   Count = city_count.Count()
                               };
            string path = "wwwroot\\TrainbyCity.csv";
            try
            {
                System.IO.StreamWriter writer;
                System.IO.FileStream file = System.IO.File.Open(path, System.IO.FileMode.OpenOrCreate);
                file.Close();
                writer = new System.IO.StreamWriter(path);
                writer.Write("City" + "," + "train\n");
                foreach (var s in trainPerCity)
                {
                    writer.Write(s.key + "," + s.Count + "\n");
                    writer.Flush();
                }
                file.Close();
                writer.Close();
            }
            catch
            {
            }

        }

        public void CountMeetingbyTypeGraph() //create data for the second graph
        {
            //calculate the amount of apartments by the number of rooms
            var MeetingbyType = from s in _context.Meetings
                                join ty in _context.TrainingTypes
                                on s.TrainingTypeID equals ty.TrainingTypeId
                                group ty by ty.Name into meetings_count
                                select new
                                {
                                    key = meetings_count.Key,
                                    Count = meetings_count.Count()
                                };
            string path = "wwwroot\\CountMeetingbyType.csv";
            try
            {
                System.IO.StreamWriter writer;
                System.IO.FileStream file = System.IO.File.Open(path, System.IO.FileMode.OpenOrCreate);
                file.Close();
                writer = new System.IO.StreamWriter(path);
                writer.Write("Training_type" + "," + "AmountOfMeetings\n");
                foreach (var a in MeetingbyType)
                {
                    writer.Write(a.key + "," + a.Count + "\n");
                    writer.Flush();
                }
                file.Close();
                writer.Close();
            }
            catch
            {
            }
        }

    }
}
