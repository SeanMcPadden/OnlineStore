using Microsoft.EntityFrameworkCore;
using TestingStripe.Models;

namespace TestingStripe.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
        {
        }

        public DbSet<CartItem> CartItems { get; set; }
        //public DbSet<Product> Product { get; set; }
    }
}
