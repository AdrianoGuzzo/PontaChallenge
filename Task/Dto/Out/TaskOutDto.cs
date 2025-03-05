using ApiTask.DataInfrastructure.Entities.Enum;

namespace ApiTask.Dto.Out
{
    public record TaskOutDto
    {
        public required string Id { get; init; }
        public required string Title { get; init; }
        public required string Description { get; init; }
        public required DateTime CreateAt { get; init; }
        public Status Status { get; init; }
    }
}
