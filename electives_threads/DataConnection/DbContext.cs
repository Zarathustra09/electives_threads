using Microsoft.EntityFrameworkCore;
using MySqlConnector;


namespace electives_threads.DataConnection
{


    public class MySqlDbContext : DbContext
    {
        public MySqlDbContext(DbContextOptions<MySqlDbContext> options) : base(options) { }
    }
}
