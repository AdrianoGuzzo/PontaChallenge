using ApiTask.DataInfrastructure.Entities.Enum;
using ApiTask.Dto.In;
using Bogus;

namespace ApiTask.Test.Mocks
{
    public class TaskInDtoFaker
    {
        private readonly Faker _faker;
        private string _title { get; init; }
        private string _description { get; init; }
        private Status _status { get; init; }

        public TaskInDtoFaker()
        {
            _faker = new Faker();
            _title = _faker.Lorem.Sentence(3);
            _description = _faker.Lorem.Paragraph();
            _status = _faker.PickRandom<Status>();
        }
        public static TaskInDtoFaker New()
            => new();

        public TaskInDto Build()
            => new()
            {
                Description = _description,
                Title = _title,
                Status = _status,
            };
    }
}
