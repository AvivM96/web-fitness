using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using web_fitness.Models;
using web_fitness.Data;
using web_fitness.Models;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace web_fitness.Controllers
{
    public class HomeController : Controller
    {
        private readonly fitnessdataContext _context;

        public HomeController(fitnessdataContext context)
        {
            _context = context;

        }
        public IActionResult Graphs()
        {
            return View();
        }
        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
     


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
