using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using electives_threads.DataConnection;
using electives_threads.Models;
using System.Security.Claims;

namespace electives_threads.Controllers
{
    public class ThreadsController : Controller
    {
        private readonly MySqlDbContext _dbContext;

        public ThreadsController(MySqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<IActionResult> Index()
        {
            var threads = await _dbContext.Threads.ToListAsync();
            return View(threads);

        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
  
        [HttpPost]
        public async Task<IActionResult> Create(electives_threads.Models.Thread thread)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the user's ID from session
                var userId = HttpContext.Session.GetInt32("UserID");

                // Check if user is authenticated
                if (userId.HasValue)
                {
                    // Set the USER_ID property of the thread
                    thread.UserID = userId.Value;

                    thread.CreatedAt = DateTime.Now;
                    thread.UpdatedAt = DateTime.Now;

                    _dbContext.Threads.Add(thread);
                    await _dbContext.SaveChangesAsync();

                    return RedirectToAction("Index", "Threads");
                }
                else
                {
                    // User is not authenticated, handle accordingly (e.g., redirect to login page)
                    return RedirectToAction("Index", "UserLogin");
                }
            }

            return View(thread);
        }
    }
}
