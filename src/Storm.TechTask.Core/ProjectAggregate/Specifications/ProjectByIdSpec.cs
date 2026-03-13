
using Ardalis.Specification;

namespace Storm.TechTask.Core.ProjectAggregate.Specifications
{
    public class ProjectByIdSpec : Specification<Project>, ISingleResultSpecification<Project>
    {
        public ProjectByIdSpec(int projectId)
        {
            this.Query.Where(project => project.Id == projectId).Include(project => project.Items);
        }
    }
}
