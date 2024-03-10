using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using electives_threads.DataConnection;
using electives_threads.Models;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authentication;

namespace electives_threads.Controllers
{
    public class UserLoginController : Controller
    {
        private readonly MySqlDbContext _dbContext;

        public UserLoginController(MySqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Username == username && u.PasswordHash == CalculateHash(password));

            if (user != null)
            {
                // Authentication successful
                // For demonstration purposes, let's store the UserID in session
                HttpContext.Session.SetInt32("UserID", user.UserID);
                return RedirectToAction("Index", "Threads"); // Redirect to home page or any other authenticated page
            }
            else
            {
                // Authentication failed
                TempData["ErrorMessage"] = "Invalid username or password";
                return RedirectToAction("Index");
            }
        }

        private string CalculateHash(string input)
        {
            // In real-world scenarios, you should use a secure hashing algorithm like bcrypt
            // This is a simple example and should not be used in production
            return input; // For simplicity, we're returning the input as hash
        }

        [HttpPost]
        public IActionResult Logout()
        {
            // Sign out the user
            HttpContext.SignOutAsync();

            // Redirect to the home page or any other page after logout
            return RedirectToAction("Index", "UserLogin");
        }
    }
}
