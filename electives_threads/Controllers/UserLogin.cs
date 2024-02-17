using Microsoft.AspNetCore.Mvc;

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
