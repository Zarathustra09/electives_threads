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

        private static int _id;

        public CommentsController(MySqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index(int threadId)
        {
            var comments = await _dbContext.Comments.Where(c => c.ThreadID == threadId).ToListAsync();

            // Fetch usernames for comment creators
            var userNames = new Dictionary<int, string>();
            foreach (var comment in comments)
            {
                var user = await _dbContext.Users.FindAsync(comment.UserID);
                userNames[comment.CommentID] = user?.Username;
            }

            // Pass the usernames to the view using ViewBag
            ViewBag.UserNames = userNames;

            return View(comments);
        }


        [HttpGet]
        public IActionResult Create()
        {
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
                    comment.ThreadID = _id;

                    // Set the CreatedAt and UpdatedAt properties
                    comment.CreatedAt = DateTime.Now;
                    comment.UpdatedAt = DateTime.Now;

                    // Add the comment to the database
                    _dbContext.Comments.Add(comment);
                    await _dbContext.SaveChangesAsync();

                    // Redirect to the index action of Threads controller
                    return RedirectToAction("Index", "Comments", new { threadId = _id });

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
        public async Task<IActionResult> Like(int commentId)
        {
            // Find the comment by commentId
            var comment = await _dbContext.Comments.FindAsync(commentId);
            if (comment != null)
            {
                // Increment the Likes count
                comment.Likes++;
                // Update the database
                await _dbContext.SaveChangesAsync();
            }

            // Redirect back to the thread's comments
            return RedirectToAction("Index", "Comments", new { threadId = _id });
        }

        [HttpPost]
        public async Task<IActionResult> Dislike(int commentId)
        {
            // Find the comment by commentId
            var comment = await _dbContext.Comments.FindAsync(commentId);
            if (comment != null)
            {
                // Increment the Dislikes count
                comment.Dislikes++;
                // Update the database
                await _dbContext.SaveChangesAsync();
            }

            // Redirect back to the thread's comments
            return RedirectToAction("Index", "Comments", new { threadId = _id });
        }
    }
}
