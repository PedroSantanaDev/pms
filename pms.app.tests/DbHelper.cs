using Microsoft.EntityFrameworkCore;
using pms.app.Data;

namespace pms.app.tests
{
    public class DbHelper
    {
        public static DbContextOptions<pms.app.Data.ApplicationDbContext> GetDbOptions()
        {
            // Set up the test database connection string
            var connectionString = "Data Source=pms.db";

            // Set up the DbContext with the test database connection string
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connectionString)
                .Options;

            return dbContextOptions;
        }
    }
}
