using Microsoft.EntityFrameworkCore;
using ToDoCs.Models;
using Microsoft.Maui.Storage;
using System.IO;

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
                var databasePath = Path.Combine(FileSystem.AppDataDirectory, "ToDoDatabase2.db");
                Directory.CreateDirectory(FileSystem.AppDataDirectory); // Ensure directory exists
                optionsBuilder.UseSqlite($"Filename={databasePath}");
            }
        }
    }
}
