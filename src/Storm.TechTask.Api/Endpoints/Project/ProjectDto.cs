using Storm.TechTask.Core.ProjectAggregate;

namespace Storm.TechTask.Api.Endpoints.Project
{
    public record ProjectDto(int Id, string Name);
    public record ProjectDetailsDto(int Id, string Name, ProjectCategory Category, ProjectStatus Status, List<ToDoItemDto> Items);

}
