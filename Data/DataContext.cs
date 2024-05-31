using dotnet8_introduction.Entities;
using dotnet8_introduction.Model;
using Microsoft.EntityFrameworkCore;

namespace dotnet8_introduction.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
