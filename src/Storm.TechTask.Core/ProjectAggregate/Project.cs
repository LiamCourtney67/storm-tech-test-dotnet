using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ardalis.GuardClauses;

using Storm.TechTask.Core.ProjectAggregate.Services;
using Storm.TechTask.SharedKernel.Entities;
using Storm.TechTask.SharedKernel.Interfaces;
using Storm.TechTask.SharedKernel.Utilities;

namespace Storm.TechTask.Core.ProjectAggregate
{
    public enum ProjectCategory
    {
        Development,
        Consultancy,
        Support,
        [EnumDescriptor(IsActive = false)]
        NotSet
    }
    public enum ProjectStatus
    {
        Open,
        Paused,
        Closed
    }

    [Serializable]
    public class Project : BaseEntity, IAggregateRoot
    {
        public string Name { get; private set; }
        public ProjectCategory Category { get; private set; }
        public bool InternalOnly { get; private set; }
        public ProjectStatus Status { get; private set; }
        public List<ToDoItem> Items { get; private set; } = new List<ToDoItem>();

        public Project(string name, ProjectCategory category, bool internalOnly, ProjectStatus status)
        {
            this.Name = name;
            this.Category = category;
            this.InternalOnly = internalOnly;
            this.Status = status;
        }

        public async Task Update(string newName, ProjectCategory newCategory, bool newInternalOnly, IProjectUniquenessChecker uniquenessChecker, CancellationToken token)
        {
            await uniquenessChecker.VerifyNameUnique(this, newName, token);
            this.Name = Guard.Against.NullOrEmpty(newName, nameof(newName));
            this.Category = Guard.Against.InvalidInput(newCategory, nameof(newCategory), c => c != ProjectCategory.NotSet);
            this.InternalOnly = newInternalOnly;
        }

        public ToDoItem AddItem(string title, string description)
        {
            var item = new ToDoItem(title, description, false);
            this.Items.Add(item);

            //var newItemAddedEvent = new NewItemAddedEvent(this, item);
            //Events.Add(newItemAddedEvent);

            return item;
        }

        public ToDoItem CompleteItem(int itemId)
        {
            var item = Items.GetChildEntity(itemId);

            item.MarkComplete();

            return item;
        }

        public void Pause()
        {
            this.Status = ProjectStatus.Paused;
        }

        public void Resume()
        {
            this.Status = ProjectStatus.Open;
        }

        public void Close()
        {
            this.Status = ProjectStatus.Closed;
        }
    }
}
