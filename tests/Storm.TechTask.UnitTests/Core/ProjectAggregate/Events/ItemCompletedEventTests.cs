using System.Linq;

using Storm.TechTask.Core.ProjectAggregate.Events;

using Xunit;
using Xunit.Abstractions;

namespace Storm.TechTask.UnitTests.Core.ProjectAggregate.Events
{
    public class ItemCompletedEventTests : BaseFixture
    {
        public ItemCompletedEventTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void RaisesItemCompletedEventWhenItemIsCompleted()
        {
            // Arrange
            var project = NewProject().Build();
            var item = project.AddItem("item", "description");

            // Act
            project.CompleteItem(item.Id);

            // Assert
            var domainEvent = Assert.Single(project.Events.OfType<ItemCompletedEvent>());
            Assert.Equal(project, domainEvent.Project);
            Assert.Equal(item, domainEvent.ItemCompleted);
            Assert.True(item.IsDone);
        }
    }
}