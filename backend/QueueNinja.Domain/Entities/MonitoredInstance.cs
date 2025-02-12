namespace QueueNinja.Domain.Entities
{

    public class MonitoredInstance
    {
        public int Id { get; set; }
        public string Name { get; set; }  // e.g., "Production Server"
        public string ConnectionString { get; set; }  // Store securely
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}