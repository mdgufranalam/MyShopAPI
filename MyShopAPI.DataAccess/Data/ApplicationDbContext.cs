using Microsoft.EntityFrameworkCore;
using ShopAPI.Models;

namespace ShopAPI.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option):base(option)
        {

        }
        public DbSet<Category> Categories { get; set; } 
        public DbSet<Product> Products { get; set; } 
    }
}
