using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using pms.app.Models;

namespace pms.app.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        // Add DbSet for the Item entity
        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            //Database.Migrate();
            //Database.EnsureCreated();
        }

        public ApplicationDbContext() : base()
        {
        }
    }
}
