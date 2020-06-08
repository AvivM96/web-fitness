using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace web_fitness.Controllers
{
    public class MeetingController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index(string eventTypeName)
        {
            ViewData["eventTypeName"] = eventTypeName;
            return View();
        }
    }
}
