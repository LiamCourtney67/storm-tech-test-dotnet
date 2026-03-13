using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Storm.TechTask.Core.ProjectAggregate;

namespace Storm.TechTask.UnitTests.Utilities.Builders.ProjectAggregate
{
    public class ProjectBuilder : BaseEntityBuilder<Project>
    {
        public ProjectBuilder() : base(new Project("name", ProjectCategory.Development, false, ProjectStatus.Open))
        {
        }

        public ProjectBuilder WithToDoItems()
        {
            return WithToDoItems(new[] {
                new ToDoItemBuilder().Set(i => i.Title, "item1").Set(i => i.Description, "111111").Set(i => i.IsDone, true).Build(),
                new ToDoItemBuilder().Set(i => i.Title, "item2").Set(i => i.Description, "222222").Set(i => i.IsDone, false).Build(),
                new ToDoItemBuilder().Set(i => i.Title, "item3").Set(i => i.Description, "333333").Set(i => i.IsDone, false).Build() });
        }

        public ProjectBuilder WithToDoItems(IList<ToDoItem> todoItems)
        {
            Target.Items.Clear();
            Target.Items.AddRange(todoItems);
            return this;
        }

        public new ProjectBuilder BuildFrom(Project toCopy)
        {
            return (ProjectBuilder)base.BuildFrom(toCopy);
        }
    }
}
