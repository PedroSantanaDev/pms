using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using pms.app.Models;

namespace pms.app.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerItem> CustomerItems { get; set; }


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
