using Storm.TechTask.Core.ProjectAggregate;

namespace Storm.TechTask.UnitTests.Utilities.Builders.ProjectAggregate
{
    public class ToDoItemBuilder : BaseEntityBuilder<ToDoItem>
    {
        public ToDoItemBuilder() : base(new ToDoItem("title", "description", false))
        {
        }
    }
}
