namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class EventLog
    {
        public string Id { get; set; }
        public string EventType { get; set; }
        public DateTime Timestamp { get; set; }
        public string IdEntity { get; set; }
    }
}
