using ApiTask.DataInfrastructure.Context.Interfaces;
using ApiTask.Exceptions;
using ApiTask.Services;
using ApiTask.Test.Mocks;
using Moq;
using System.Linq.Expressions;

namespace ApiTask.Test.Services
{
    public class TaskServiceTest
    {
        private readonly Mock<ITaskContext> _mockDbContext;
        private readonly TaskService _taskService;

        public TaskServiceTest()
        {
            _mockDbContext = new Mock<ITaskContext>();
            _taskService = new TaskService(_mockDbContext.Object);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetById_TaskFound_ReturnsTaskDto()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            DataInfrastructure.Entities.Task taskEntity = TaskFaker.New().WithId(taskId).Build();

            _mockDbContext
                .Setup(db => db.SingleOrDefaultAsync(It.IsAny<Expression<Func<DataInfrastructure.Entities.Task, bool>>>()))
                .ReturnsAsync(taskEntity);

            // Act
            var result = await _taskService.GetById(taskId.ToString());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(taskEntity.Id, taskId);        
            Assert.Equal(taskEntity.Title, result.Title);
            Assert.Equal(taskEntity.Description, result.Description);
            Assert.Equal(taskEntity.Status, result.Status);
        }

        [Fact]
        public async System.Threading.Tasks.Task Create_ValidDto_CreatesTaskAndReturnsDto()
        {
            // Arrange
            var taskInDto = TaskInDtoFaker.New().Build();

            var userId = Guid.NewGuid().ToString();
            var taskEntity =  DataInfrastructure.Entities.Task.New(taskInDto, userId);

            _mockDbContext.Setup(db => db.AddAsync<DataInfrastructure.Entities.Task>(It.IsAny<DataInfrastructure.Entities.Task>(), It.IsAny<CancellationToken>()));
            _mockDbContext.Setup(db => db.SaveChanges()).Returns(1);

            // Act
            var result = await _taskService.Create(taskInDto, userId);

            // Assert
            Assert.NotNull(result);     
            Assert.Equal(taskEntity.Title, result.Title);
            Assert.Equal(taskEntity.Description, result.Description);
            Assert.Equal(taskEntity.Status, result.Status);
        }

        [Fact]
        public async System.Threading.Tasks.Task Update_ValidUser_UpdatesTaskAndReturnsDto()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var taskInDto = TaskInDtoFaker.New().Build();

            var userId = Guid.NewGuid();
            var taskEntity = TaskFaker.New().WithUserId(userId).WithId(taskId).Build();

            _mockDbContext
                .Setup(db => db.SingleOrDefaultAsync(It.IsAny<Expression<Func<DataInfrastructure.Entities.Task, bool>>>()))
                .ReturnsAsync(taskEntity);
            _mockDbContext.Setup(db => db.SaveChanges()).Returns(1);

            // Act
            var result = await _taskService.Update(taskId.ToString(), taskInDto, userId.ToString());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(taskEntity.Title, result.Title);
            Assert.Equal(taskEntity.Description, result.Description);
            Assert.Equal(taskEntity.Status, result.Status);
        }

        [Fact]
        public async System.Threading.Tasks.Task Update_InvalidUser_ThrowsForbiddenAccessException()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var taskInDto = TaskInDtoFaker.New().Build();

            var userId = Guid.NewGuid();
            var taskEntity = TaskFaker.New().WithId(taskId).Build();

            _mockDbContext
                .Setup(db => db.SingleOrDefaultAsync(It.IsAny<Expression<Func<DataInfrastructure.Entities.Task, bool>>>()))
                .ReturnsAsync(taskEntity);     

            // Act & Assert
            await Assert.ThrowsAsync<ForbiddenAccessException>(() => _taskService.Update(taskId.ToString(), taskInDto, userId.ToString()));
        }

    }

}
