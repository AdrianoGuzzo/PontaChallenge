using ApiTask.DataInfrastructure.Entities.Enum;
using System.ComponentModel.DataAnnotations;

namespace ApiTask.Dto.In
{
    public record TaskInDto
    {
        [MaxLength(100)]
        public required string Title { get; init; }
        [MaxLength(255)]
        public required string Description { get; init; }
        [EnumDataType(typeof(Status), ErrorMessage = "O valor do Status não é válido.")]
        public Status Status { get; init; }
    }
}
