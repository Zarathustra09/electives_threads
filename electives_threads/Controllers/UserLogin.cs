using electives_threads.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace electives_threads.Controllers
{
    public class UserLogin : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/UserLogin/Index.cshtml");
        }
    }
}
