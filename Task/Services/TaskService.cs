using ApiTask.DataInfrastructure.Context;
using ApiTask.DataInfrastructure.Entities.Enum;
using ApiTask.Dto.In;
using ApiTask.Dto.Out;
using ApiTask.Exceptions;
using ApiTask.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiTask.Services
{
    public class TaskService(TaskDbContext taskDbContext) : ITaskService
    {
        private readonly TaskDbContext _taskDbContext = taskDbContext;

        public async Task<TaskOutDto> GetById(string id)
        {
            var taskEntity = await GetEntityById(id);

            return taskEntity.GetToDto();
        }
        public async Task<TaskOutDto[]> GetList(Status? status)
        {
            var query = _taskDbContext.Task.AsQueryable();

            if (status is not null)
                query = query.Where(task => task.Status == status);

            var list = await query.AsNoTracking().ToArrayAsync();

            return list.Select(task => task.GetToDto()).ToArray();
        }

        public async Task<bool> Create(TaskInDto taskCreateDto, string userId)
        {
            var taskEntity = DataInfrastructure.Entities.Task.New(taskCreateDto, userId);
            await _taskDbContext.AddAsync(taskEntity);
            _taskDbContext.SaveChanges();
            return true;
        }

        public async Task<bool> Update(string id, TaskInDto taskIUpdateDto, string userId)
        {
            var userIdGuid = Guid.Parse(userId);
            var taskEntity = await GetEntityById(id);

            if (!taskEntity.UserId.Equals(userIdGuid))
                throw new ForbiddenAccessException("Usuário não autorizado para atualizar");

            taskEntity.UpdateFromDto(taskIUpdateDto);
            _taskDbContext.SaveChanges();
            return true;
        }

        public async Task<bool> Delete(string id, string userId)
        {
            var userIdGuid = Guid.Parse(userId);
            var taskEntity = await GetEntityById(id);

            if (!taskEntity.UserId.Equals(userIdGuid))
                throw new ForbiddenAccessException("Usuário não autorizado para deletar");

            _taskDbContext.Task.Remove(taskEntity);
            _taskDbContext.SaveChanges();
            return true;
        }

        private async Task<DataInfrastructure.Entities.Task> GetEntityById(string id)
        {
            var taskEntity = await _taskDbContext.Task.SingleOrDefaultAsync(task => task.Id.ToString() == id);
            if (taskEntity is null)
                throw new KeyNotFoundException(id);
            return taskEntity;

        }
    }
}
