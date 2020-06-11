using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web_fitness.Data;
using web_fitness.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace web_fitness.Controllers
{
    public class TrainerController : Controller
    {

        private readonly fitnessdataContext _context;

        public TrainerController(fitnessdataContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Details(string id)
        {
            ViewData["test"] = id;
            var trainer = await _context.Trainers.SingleOrDefaultAsync(t => t.TrainerName == "Aviv Test");
            return View(trainer);
        }
    }
}

