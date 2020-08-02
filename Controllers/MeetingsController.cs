using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web_fitness.Data;
using TweetSharp;
using web_fitness.MeetingsClusterer;
using web_fitness.Models;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace web_fitness.Controllers
{
    public class MeetingsController : Controller
    {
        private readonly fitnessdataContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Clusterer _clusterer;

        public MeetingsController(fitnessdataContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _clusterer = new Clusterer(_context);
            _clusterer.CreateModel();
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
                return View("~/Views/Home/Index.cshtml");
            }

            var meeting = await _context.Meetings
                .Include(m => m.TrainType)
                .Include(m => m.Trainer)
                .FirstOrDefaultAsync(m => m.MeetID ==id );

            if (meeting == null)
            {
                return View("~/Views/Home/Index.cshtml");
            }

            if (User.IsInRole("Trainer") && _userManager.GetUserId(User) != meeting.TrainerID)
            {
                ViewData["AccessDenied"] = true;
            }


            try
            {
                MeetingPrediction prediction = _clusterer.Predict(meeting);
                List<Meeting> MeetingsInSameCluster = await GetMeetingsInCluster(prediction.PredictedClusterId);
                MeetingsInSameCluster.Remove(meeting);
                ViewBag.otherMeetings = MeetingsInSameCluster;
            }
            catch
            {
                Console.Write("failed to cluster");
            }


            return View(meeting);
        }

        // GET: Meetings/Create
        [Authorize(Roles ="Admin, Trainer")]
        public IActionResult Create()
        {
            ViewData["TrainingTypeID"] = new SelectList(_context.TrainingTypes, "TrainingTypeId", "Name");
            ViewData["TrainerID"] = this.GetRelevantTrainersToSelect();
            return View();
        }

        // POST: Meetings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, Trainer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MeetID,TrainingTypeID,TrainerID,MeetDate,Price")] Meeting meeting, string oauth_token, string oauth_verifier)
        {

            string key = "r6tXd2oaTFEqADpqI7GwsiR5o";
            string secret = "6vqMDzhw0KmSiHXa7VXGARMc8FyEBIxK6EK52XyA6EopEIos4H";
            string token = "1288845984898994179-vGxfZsfSKO2RpkQpdD3KcCTzoze9C1";
            string tokenSecret = "9xRofqqaVXfTaF8lChqBFlWbHwnnsdy9wN3xCPyr7BPRQ";
            var service = new TweetSharp.TwitterService(key, secret);
            service.AuthenticateWith(token, tokenSecret);
            TwitterUser user = service.VerifyCredentials(new VerifyCredentialsOptions());
            var traintype = await _context.TrainingTypes
               .FirstOrDefaultAsync(m => m.TrainingTypeId == meeting.TrainingTypeID);
            string message = string.Format("New {0} meeting is available at {1}", traintype.Name, meeting.MeetDate.Date);
            var result = service.SendTweet(new SendTweetOptions
            {
                Status = message
            });

            if (ModelState.IsValid)
            {
                _context.Add(meeting);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TrainingTypeID"] = new SelectList(_context.TrainingTypes, "TrainingTypeId", "Name");
            ViewData["TrainerID"] = this.GetRelevantTrainersToSelect();
            return View(meeting);
        }

        // GET: Meetings/Edit/5

        [Authorize(Roles = "Admin, Trainer")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("~/Views/Home/Index.cshtml");
            }

            var meeting = await _context.Meetings
                .Include(m => m.TrainType)
                .Include(m => m.Trainer)
                .FirstOrDefaultAsync(m => m.MeetID == id);

             if (meeting == null)
            {
                return View("~/Views/Home/Index.cshtml");
            }

            ViewData["TrainingTypeID"] = new SelectList(_context.TrainingTypes, "TrainingTypeId", "Name");
            ViewData["TrainerID"] = this.GetRelevantTrainersToSelect();
            return View(meeting);
        }

        // POST: Meetings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, Trainer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MeetID,TrainingTypeID,TrainerID,MeetDate,Price")] Meeting meeting)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if(User.IsInRole("Trainer") && meeting.TrainerID != this._userManager.GetUserId(User))
                    {
                        ViewData["AccessDenied"] = true;
                        return View(meeting);
                    }

                    _context.Update(meeting);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MeetingExists(meeting.MeetID))
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
            ViewData["TrainingTypeID"] = new SelectList(_context.TrainingTypes, "TrainingTypeId", "Name", meeting.TrainingTypeID);
            ViewData["TrainerID"] = this.GetRelevantTrainersToSelect();
            return View(meeting);
        }

        // DELETE: Meetings/Delete/5
        [Authorize(Roles = "Admin, Trainer")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("~/Views/Home/Index.cshtml");
            }

            var meeting = await _context.Meetings
                .Include(m => m.TrainType)
                .Include(m => m.Trainer)
                .FirstOrDefaultAsync(m => m.MeetID == id);
            if (meeting == null)
            {
                return View("~/Views/Home/Index.cshtml");
            }

            return View(meeting);
        }

        public async Task<IActionResult> List(string eventTypeName)
        {
            ViewData["eventTypeName"] = eventTypeName;
            var meetings_of_type = await _context.Meetings.Include(m => m.Trainer).Include(m => m.TrainType).Where(m => m.TrainType.Name == eventTypeName).ToListAsync();
            return View(meetings_of_type);
        }

        // POST: Meetings/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin, Trainer")]
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
        public async Task<IActionResult> MultipleSearch(DateTime startDate, string city, string price, string typename)
        {

            var meeting = from s in _context.Meetings
                           join a in _context.AspNetUsers
                           on s.TrainerID equals a.Id
                           join t in _context.TrainingTypes
                           on s.TrainingTypeID equals t.TrainingTypeId
                           select new
                           { a.City,t.TrainingTypeId,t.Name,s.MeetDate,s.MeetID,a.Email,s.Price,a.LastName,a.FirstName,a.Id};

            if (!(startDate == DateTime.MinValue) || !String.IsNullOrEmpty(city) || !String.IsNullOrEmpty(price) || !String.IsNullOrEmpty(typename))
            {
                if (!String.IsNullOrEmpty(typename))
                {
                    meeting = meeting.Where(s => s.Name.Contains(typename));
                }
                if (!String.IsNullOrEmpty(city))
                {
                    meeting = meeting.Where(s => s.City.Contains(city));
                }
                if (!String.IsNullOrEmpty(price))
                {
                    meeting = meeting.Where(s => s.Price <= Int32.Parse(price));
                }
                if (!startDate.Equals(DateTime.MinValue))
                {
                    meeting = meeting.Where(s => s.MeetDate <= startDate);
                }

            }

            var ids = meeting.Select(s => s.MeetID);
            List<Meeting> MeetingsSearch = await _context.Meetings.Include(m => m.Trainer).Include(m => m.TrainType).Where(s => ids.Contains(s.MeetID)).ToListAsync();



            return View("~/Views/Meetings/Index.cshtml", MeetingsSearch);
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
        async private Task<List<Meeting>> GetMeetingsInCluster(uint clusterId)
        {
            List<Meeting> meetings_in_same_cluster = new List<Meeting>();
            foreach (Meeting m in await _context.Meetings.ToListAsync())
            {
                try
                {
                    MeetingPrediction meetingPrediction = _clusterer.Predict(m);
                    if (meetingPrediction.PredictedClusterId == clusterId)
                        meetings_in_same_cluster.Add(m);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return meetings_in_same_cluster;
        }

        private SelectList GetRelevantTrainersToSelect() {
            if (User.IsInRole("Trainer"))
            {
                var currentUser = _context.AspNetUsers.Find(this._userManager.GetUserId(User));

                return new SelectList(new List<ApplicationUser> { currentUser }, "Id", "Email");
            }

            return new SelectList(_context.AspNetUsers.Where(t => t.IsTrainer).ToList(), "Id", "Email");
        }

    }
}
