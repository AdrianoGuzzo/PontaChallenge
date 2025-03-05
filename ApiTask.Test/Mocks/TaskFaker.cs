using ApiTask.DataInfrastructure.Entities.Enum;
using Bogus;
using System.ComponentModel.DataAnnotations;

namespace ApiTask.Test.Mocks
{
    public class TaskFaker
    {
        private readonly Faker _faker;
        private Guid _id { get; set; }
        private Guid _userId { get; set; }
        private string _title { get; set; }
        private string _description { get; set; }
        private DateTime _createAt { get; set; }
        private Status _status { get; set; }
        private TaskFaker()
        {
            _faker = new Faker();
            _id = Guid.NewGuid();
            _userId = Guid.NewGuid();
            _title = _faker.Lorem.Sentence(3);
            _description = _faker.Lorem.Paragraph();
            _createAt = _faker.Date.Recent();
            _status = _faker.PickRandom<Status>();
        }

        public static TaskFaker New()
            => new();

        public TaskFaker WithId(Guid id)
        {
            _id = id;
            return this;
        }
        public TaskFaker WithUserId(Guid userId)
        {
            _userId = userId;
            return this;
        }

        public DataInfrastructure.Entities.Task Build()
            => new(_id,
                _userId,
                _title,
                _description,
                _createAt,
                _status);
    }
}
