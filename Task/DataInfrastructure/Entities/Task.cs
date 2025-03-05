using ApiTask.DataInfrastructure.Entities.Enum;
using ApiTask.Dto.In;
using ApiTask.Dto.Out;
using System.ComponentModel.DataAnnotations;

namespace ApiTask.DataInfrastructure.Entities
{
    public class Task
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        [MaxLength(100)]
        public string Title { get; private set; }
        [MaxLength(255)]
        public string Description { get; private set; }
        public DateTime CreateAt { get; private set; }
        public Status Status { get; private set; }

        public Task(Guid id, Guid userId, string title, string description, DateTime createAt, Status status)
        {
            Id = id;
            Title = title;
            Description = description;
            CreateAt = createAt;
            Status = status;
            UserId = userId;
        }

        public static Task New(TaskInDto taskCreateDto, string userId)
        => new(Guid.NewGuid(), Guid.Parse(userId), taskCreateDto.Title, taskCreateDto.Description, DateTime.Now, taskCreateDto.Status);

        public void UpdateFromDto(TaskInDto taskCreateDto)
        {
            Title = taskCreateDto.Title;
            Description = taskCreateDto.Description;
            Status = taskCreateDto.Status;
        }
        public TaskOutDto GetToDto()
            => new()
            {
                Id = Id.ToString(),
                CreateAt = CreateAt,
                Description = Description,
                Status = Status,
                Title = Title,
            };
    }
}
