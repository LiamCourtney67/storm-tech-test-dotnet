using MediatR;

using Microsoft.AspNetCore.Mvc;

using Storm.TechTask.Core.ProjectAggregate;
using Storm.TechTask.Core.ProjectAggregate.Queries;
using Storm.TechTask.SharedKernel.Interfaces;

using Swashbuckle.AspNetCore.Annotations;

namespace Storm.TechTask.Api.Endpoints.Project
{
    public class GetByIdEndpoint : BaseEndpoint
        .WithRequest<ProjectDetails.Query>
        .WithResponse<ProjectDto>
    {
        public GetByIdEndpoint(IMediator mediator, ILoggingService loggingService, ISecurityService securityService) : base(mediator, loggingService, securityService)
        {
        }

        [HttpGet("/Projects/{Id:int}")]
        [SwaggerOperation(
            Summary = "Gets a single Project",
            Description = "Gets a single Project by Id",
            OperationId = "Projects.GetById",
            Tags = new[] { "ProjectEndpoints" })
        ]
        public override async Task<ActionResult<ProjectDto>> HandleAsync([FromRoute] ProjectDetails.Query request,
            CancellationToken cancellationToken)
        {
            var entity = await _mediator.Send(request, cancellationToken);
            if (entity == null)
            {
                return NotFound();
            }

            var response = new ProjectDetailsDto(
                entity.Id,
                entity.Name,
                entity.Category,
                entity.Status,
                entity.Items
                    .OrderBy(i => i.Id)
                    .Select(i => new ToDoItemDto(i.Id, i.Title, i.Description, i.IsDone))
                    .ToList()
            );

            return Ok(response);
        }
    }
}
