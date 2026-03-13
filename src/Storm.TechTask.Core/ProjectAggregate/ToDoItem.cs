using Storm.TechTask.SharedKernel.Entities;

namespace Storm.TechTask.Core.ProjectAggregate
{
    [Serializable]
    public class ToDoItem : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsDone { get; private set; }

        public ToDoItem()
        {
        }

        public ToDoItem(string title, string description, bool isDone)
        {
            this.Title = title;
            this.Description = description;
            this.IsDone = isDone;
        }

        public void MarkComplete()
        {
            this.IsDone = true;

            // This would be a good place for another domain event
        }

        public override string ToString()
        {
            string status = this.IsDone ? "Done!" : "Not done.";
            return $"{Id}: Status: {status} - {Title} - {Description}";
        }
    }
}
