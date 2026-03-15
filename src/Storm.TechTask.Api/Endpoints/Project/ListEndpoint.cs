using MediatR;

using Microsoft.AspNetCore.Mvc;

using Storm.TechTask.Core.ProjectAggregate.Queries;
using Storm.TechTask.SharedKernel.Interfaces;

using Swashbuckle.AspNetCore.Annotations;

namespace Storm.TechTask.Api.Endpoints.Project
{
    public class ListEndpoint : BaseEndpoint
        .WithRequest<AllProjects.Query>
        .WithResponse<List<ProjectDto>>
    {
        public ListEndpoint(IMediator mediator, ILoggingService loggingService, ISecurityService securityService) : base(mediator, loggingService, securityService)
        {
        }

        [HttpGet("/Projects")]
        [ProducesResponseType(typeof(List<ProjectDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [SwaggerOperation(
            Summary = "Gets a list of all Projects",
            Description = "Gets a list of all Projects",
            OperationId = "Projects.List",
            Tags = new[] { "ProjectEndpoints" })
        ]
        public override async Task<ActionResult<List<ProjectDto>>> HandleAsync([FromRoute] AllProjects.Query request, CancellationToken cancellationToken)
        {
            var projects = (await _mediator.Send(request, cancellationToken))
                .Select(project => new ProjectDto(project.Id, project.Name))
                .ToList();

            return Ok(projects);
        }
    }
}
