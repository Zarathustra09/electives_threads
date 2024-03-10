using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using electives_threads.DataConnection;
using electives_threads.Models;

namespace electives_threads.Controllers
{
    public class CommentsController : Controller
    {
        private readonly MySqlDbContext _dbContext;

        public CommentsController(MySqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            List<Comment> comments = await _dbContext.Comments.ToListAsync();
            return View(comments);
        }


        [HttpGet]
        public IActionResult Create(int threadId)
        {
            ViewBag.ThreadID = threadId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Comment comment)
        {
            if (ModelState.IsValid)
            {
                // Get the UserID from the session
                var userId = HttpContext.Session.GetInt32("UserID");

                // Check if user is authenticated
                if (userId.HasValue)
                {
                    // Set the UserID and ThreadID for the comment
                    comment.UserID = userId.Value;
                    comment.ThreadID = ViewBag.ThreadID;

                    // Set the CreatedAt and UpdatedAt properties
                    comment.CreatedAt = DateTime.Now;
                    comment.UpdatedAt = DateTime.Now;

                    // Add the comment to the database
                    _dbContext.Comments.Add(comment);
                    await _dbContext.SaveChangesAsync();

                    // Redirect to the index action of Threads controller
                    return RedirectToAction("Index", "Threads");
                }
                else
                {
                    // User is not authenticated, handle accordingly (e.g., redirect to login page)
                    return RedirectToAction("Index", "UserLogin");
                }
            }

            return View(comment);
        }

        [HttpPost]
        public async Task<IActionResult> Like(int threadId)
        {
            // Implement like functionality
            return RedirectToAction("Index", "Threads");
        }

        [HttpPost]
        public async Task<IActionResult> Dislike(int threadId)
        {
            // Implement dislike functionality
            return RedirectToAction("Index", "Threads");
        }
    }
}
