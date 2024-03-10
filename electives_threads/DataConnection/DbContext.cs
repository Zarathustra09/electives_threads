using Microsoft.EntityFrameworkCore;
using electives_threads.Models;

namespace electives_threads.DataConnection
{
    public class MySqlDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<electives_threads.Models.Thread> Threads { get; set; } // Add this line to define the Threads DbSet

        public DbSet<Comment> Comments { get; set; }

        public MySqlDbContext(DbContextOptions<MySqlDbContext> options) : base(options) { }
    }
}
