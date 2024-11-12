using Microsoft.EntityFrameworkCore;
using ToDoCs.Models;

namespace ToDoCs.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<ToDoItem> ToDoItems { get; set; }

        // Parameterless constructor for design-time
        public AppDbContext()
        {
        }

        // Constructor used at runtime
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Provide a fixed database path for design-time
                optionsBuilder.UseSqlite("Filename=ToDoDatabase2.db");
            }
        }

        // Remove the OnModelCreating method if it only contained default value configurations
    }
}
