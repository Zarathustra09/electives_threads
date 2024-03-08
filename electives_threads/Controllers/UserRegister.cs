using Microsoft.AspNetCore.Mvc;

namespace electives_threads.Controllers
{
    public class UserRegister : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
