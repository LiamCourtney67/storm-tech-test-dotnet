using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Storm.TechTask.SharedKernel.Entities;

namespace Storm.TechTask.Core.ProjectAggregate.Events
{
    public class NewItemAddedEvent : BaseDomainEvent
    {
        public ToDoItem NewItem { get; set; }
        public Project Project { get; set; }

        public NewItemAddedEvent(Project project,
            ToDoItem newItem)
        {
            Project = project;
            NewItem = newItem;
        }
    }
}
