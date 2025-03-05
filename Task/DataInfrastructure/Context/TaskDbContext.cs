using Microsoft.EntityFrameworkCore;

namespace ApiTask.DataInfrastructure.Context
{
    public class TaskDbContext(DbContextOptions<TaskDbContext> options) : DbContext(options)
    {
        public DbSet<Entities.Task> Task { get; set; }
    }
}
