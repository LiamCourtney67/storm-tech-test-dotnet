using MediatR;

using Storm.TechTask.Core.ProjectAggregate.Events;
using Storm.TechTask.SharedKernel.Interfaces;

namespace Storm.TechTask.Core.ProjectAggregate.EventHandlers
{
    public class NewItemAddedEventHandler : INotificationHandler<NewItemAddedEvent>
    {
        private readonly ILoggingService _loggingService;

        public NewItemAddedEventHandler(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public Task Handle(NewItemAddedEvent notification, CancellationToken cancellationToken)
        {
            _loggingService.CommonLogger.Information(
                "ToDo item added to project {ProjectId}: {ItemTitle}",
                notification.Project.Id,
                notification.NewItem.Title);

            return Task.CompletedTask;
        }
    }
}