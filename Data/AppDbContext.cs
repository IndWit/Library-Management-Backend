using Microsoft.EntityFrameworkCore;
using Library_Management_System.Models; // <--- This matches your error log name

namespace Library_Management_System.Data
{
    public class AppDbContext : DbContext
    {
        // This "Constructor" is what was missing or broken
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
    }
}