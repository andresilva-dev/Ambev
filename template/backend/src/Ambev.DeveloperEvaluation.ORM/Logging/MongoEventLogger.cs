using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Services;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.ORM.Logging
{
    public class MongoEventLogger : IEventLogger
    {
        private readonly IMongoCollection<EventLog> _collection;

        public MongoEventLogger(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MongoDb");
            var mongoUrl = new MongoUrl(configuration.GetConnectionString("MongoDb"));
            var client = new MongoClient(mongoUrl);
            var database = client.GetDatabase("Logs");
            _collection = database.GetCollection<EventLog>("EventLogs");
        }

        public async Task LogAsync(string eventType, string id, CancellationToken cancellationToken = default)
        {
            var log = new EventLog
            {
                Id = Guid.NewGuid().ToString(),
                EventType = eventType,
                Timestamp = DateTime.UtcNow,
                IdEntity = id
            };

            await _collection.InsertOneAsync(log, null, cancellationToken);
        }
    }
}
