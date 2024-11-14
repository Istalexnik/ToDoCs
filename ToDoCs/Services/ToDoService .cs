using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoCs.Data;
using ToDoCs.Models;

namespace ToDoCs.Services
{
    public class ToDoService
    {
        private readonly AppDbContext _dbContext;

        public ToDoService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddToDoItemAsync(ToDoItem item)
        {
            _dbContext.ToDoItems.Add(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ToDoItem?> GetToDoItemByIdAsync(int id)
        {
            return await _dbContext.ToDoItems.FindAsync(id);
        }

        public async Task DeleteToDoItemAsync(ToDoItem item)
        {
            _dbContext.ToDoItems.Remove(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateToDoItemAsync(ToDoItem item)
        {
            _dbContext.ToDoItems.Update(item);
            await _dbContext.SaveChangesAsync();
        }

        // New method to retrieve all items from the database
        public async Task<List<ToDoItem>> GetAllToDoItemsAsync()
        {
            return await _dbContext.ToDoItems.ToListAsync();
        }
    }
}
