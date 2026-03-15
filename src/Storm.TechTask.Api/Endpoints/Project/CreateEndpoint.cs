using MediatR;

using Microsoft.AspNetCore.Mvc;

using Storm.TechTask.Core.ProjectAggregate.Commands;
using Storm.TechTask.SharedKernel.Interfaces;

using Swashbuckle.AspNetCore.Annotations;

namespace Storm.TechTask.Api.Endpoints.Project
{
    public class CreateEndpoint : BaseEndpoint
        .WithRequest<CreateProject.Command>
        .WithResponse<ProjectDto>
    {
        public CreateEndpoint(IMediator mediator, ILoggingService loggingService, ISecurityService securityService) : base(mediator, loggingService, securityService)
        {
        }

        [HttpPost("/Projects")]
        [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]     // May also return ValidationProblemDetails, ProblemDetails is returned for a BusinessRuleException 
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [SwaggerOperation(
            Summary = "Creates a new Project",
            Description = "Creates a new Project",
            OperationId = "Projects.Create",
            Tags = new[] { "ProjectEndpoints" })]
        public override async Task<ActionResult<ProjectDto>> HandleAsync(CreateProject.Command request,
            CancellationToken cancellationToken)
        {
            var project = await _mediator.Send(request, cancellationToken);

            return Created($"/Projects/{project.Id}", new ProjectDto(project.Id, project.Name));
        }
    }
}
