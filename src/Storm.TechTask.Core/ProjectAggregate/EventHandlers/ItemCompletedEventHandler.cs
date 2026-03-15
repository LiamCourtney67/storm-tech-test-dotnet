using MediatR;

using Storm.TechTask.Core.ProjectAggregate.Events;
using Storm.TechTask.SharedKernel.Interfaces;

namespace Storm.TechTask.Core.ProjectAggregate.EventHandlers
{
    public class ItemCompletedEventHandler : INotificationHandler<ItemCompletedEvent>
    {
        private readonly ILoggingService _loggingService;

        public ItemCompletedEventHandler(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public Task Handle(ItemCompletedEvent notification, CancellationToken cancellationToken)
        {
            _loggingService.CommonLogger.Information(
                "ToDo item completed in project {ProjectId}: {ItemTitle}",
                notification.Project.Id,
                notification.ItemCompleted.Title);

            return Task.CompletedTask;
        }
    }
}