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
    public class CompletedItemEventHandlerTests : BaseFixture
    {
        public CompletedItemEventHandlerTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task LogsWhenItemIsCompleted()
        {
            // Arrange
            var logger = new Mock<ILogger>();
            var loggingService = new Mock<ILoggingService>();
            loggingService.Setup(x => x.CommonLogger).Returns(logger.Object);

            var project = NewProject().Build();
            var item = project.AddItem("item", "description");
            item.MarkComplete();

            var notification = new ItemCompletedEvent(project, item);
            var handler = new ItemCompletedEventHandler(loggingService.Object);

            // Act
            await handler.Handle(notification, default);

            // Assert
            logger.Verify(x =>
                x.Information(
                    "ToDo item completed in project {ProjectId}: {ItemTitle}",
                    project.Id,
                    item.Title),
                Times.Once);
        }
    }
}