using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using web_fitness.Data;
using web_fitness.Models;

//For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace web_fitness.Controllers
{
    public class MeetingController : Controller
    {
        private readonly fitnessdataContext _context;

        public MeetingController(fitnessdataContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index(string eventTypeName)
        {
            ViewData["eventTypeName"] = eventTypeName;
            return View();
        }

        public async Task<IActionResult> List(string eventTypeName)
        {
            var meetings_of_type = await _context.Meetings.Where(m => m.TrainType.Name == eventTypeName).ToListAsync();
            return View(meetings_of_type);
        }
    }
}
