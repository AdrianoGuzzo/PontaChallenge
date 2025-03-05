using ApiTask.DataInfrastructure.Entities.Enum;
using ApiTask.Dto.In;
using ApiTask.Dto.Out;

namespace ApiTask.Services.Interfaces
{
    public interface ITaskService
    {
        Task<TaskOutDto> GetById(string id);
        Task<TaskOutDto[]> GetList(Status? status);
        Task<bool> Create(TaskInDto taskCreateDto, string userId);
        Task<bool> Update(string id, TaskInDto taskUpdateDto, string userId);
        Task<bool> Delete(string id, string userId);
    }
}
