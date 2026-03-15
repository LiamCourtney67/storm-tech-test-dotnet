using Moq;

using Serilog;

using Storm.TechTask.Core.ProjectAggregate;
using Storm.TechTask.Core.ProjectAggregate.EventHandlers;
using Storm.TechTask.Core.ProjectAggregate.Events;
using Storm.TechTask.SharedKernel.Interfaces;

using Xunit;
using Xunit.Abstractions;

namespace Storm.TechTask.UnitTests.Core.ProjectAggregate.EventHandlers
{
    public class NewItemAddedEventHandlerTests : BaseFixture
    {
        public NewItemAddedEventHandlerTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task LogsWhenNewItemIsAdded()
        {
            // Arrange
            var logger = new Mock<ILogger>();
            var loggingService = new Mock<ILoggingService>();
            loggingService.Setup(x => x.CommonLogger).Returns(logger.Object);

            var project = NewProject().Build();
            var item = project.AddItem("item", "description");

            var notification = new NewItemAddedEvent(project, item);
            var handler = new NewItemAddedEventHandler(loggingService.Object);

            // Act
            await handler.Handle(notification, default);

            // Assert
            logger.Verify(x =>
                x.Information(
                    "ToDo item added to project {ProjectId}: {ItemTitle}",
                    project.Id,
                    item.Title),
                Times.Once);
        }
    }
}