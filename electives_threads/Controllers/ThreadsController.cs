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

            // Fetch usernames for thread creators
            var userNames = new Dictionary<int, string>();
            foreach (var thread in threads)
            {
                var user = await _dbContext.Users.FindAsync(thread.UserID);
                userNames[thread.ThreadID] = user?.Username;
            }

            // Pass the usernames to the view using ViewBag
            ViewBag.UserNames = userNames;

            return View(threads);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(electives_threads.Models.Thread thread, IFormFile photo)
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

                    // Handle photo upload
                    if (photo != null && photo.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await photo.CopyToAsync(memoryStream);
                            thread.PhotoData = memoryStream.ToArray();
                            thread.PhotoFileName = photo.FileName;
                        }
                    }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Like(int id)
        {
            var thread = await _dbContext.Threads.FindAsync(id);
            if (thread == null)
            {
                return NotFound();
            }

            // Increase the likes count
            thread.Likes++;

            _dbContext.Update(thread);
            await _dbContext.SaveChangesAsync();

            // Redirect back to the thread index page
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dislike(int id)
        {
            var thread = await _dbContext.Threads.FindAsync(id);
            if (thread == null)
            {
                return NotFound();
            }

            // Increase the dislikes count
            thread.Dislikes++;

            _dbContext.Update(thread);
            await _dbContext.SaveChangesAsync();

            // Redirect back to the thread index page
            return RedirectToAction("Index");
        }

    }
}
