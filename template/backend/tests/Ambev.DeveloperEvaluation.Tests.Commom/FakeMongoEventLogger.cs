using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Services;

namespace Ambev.DeveloperEvaluation.Tests.Commom
{
    public class FakeMongoEventLogger : IEventLogger
    {
        private readonly List<EventLog> _logs = new();

        public Task LogAsync(string eventType, string idEntity, CancellationToken cancellationToken = default)
        {
            _logs.Add(new EventLog
            {
                EventType = eventType,
                IdEntity = idEntity
            });

            return Task.CompletedTask;
        }

    }
}
