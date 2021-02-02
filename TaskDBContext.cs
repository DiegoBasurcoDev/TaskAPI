using Microsoft.EntityFrameworkCore;
using Models;

namespace TaskAPI
{
    public class TaskDBContext : DbContext
    {
        public TaskDBContext(DbContextOptions<TaskDBContext> options) : base(options)
        {

        }
        public DbSet<Task> Task { get; set; }
        public DbSet<Author> Author { get; set; }
    }
}