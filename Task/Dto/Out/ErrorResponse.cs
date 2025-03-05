namespace ApiTask.Dto.Out
{
    public record ErrorResponse
    {
        public required string[] Messages { get; init; }
    }
}
