using System.Linq;

using Storm.TechTask.Core.ProjectAggregate.Events;

using Xunit;
using Xunit.Abstractions;

namespace Storm.TechTask.UnitTests.Core.ProjectAggregate.Events
{
    public class NewItemAddedEventTests : BaseFixture
    {
        public NewItemAddedEventTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void RaisesNewItemAddedEventWhenItemIsAdded()
        {
            // Arrange
            var project = NewProject().Build();

            // Act
            var item = project.AddItem("item", "description");

            // Assert
            var domainEvent = Assert.Single(project.Events.OfType<NewItemAddedEvent>());
            Assert.Equal(project, domainEvent.Project);
            Assert.Equal(item, domainEvent.NewItem);
        }
    }
}