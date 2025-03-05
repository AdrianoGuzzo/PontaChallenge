using ApiTask.DataInfrastructure.Context.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq.Expressions;

namespace ApiTask.DataInfrastructure.Context
{
    public class TaskDbContext(DbContextOptions<TaskDbContext> options) : DbContext(options), ITaskContext
    {
        private DbSet<Entities.Task> Task { get; init; }

        public async Task<TEntity?> SingleOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
            => await Set<TEntity>().SingleOrDefaultAsync(predicate);

        public IQueryable<TEntity> AsQueryable<TEntity>() where TEntity : class
            => Set<TEntity>().AsQueryable();       
    }
}
