namespace QueueNinja.Domain.Entities
{
    public class Tenant
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<UserTenant> UserTenants { get; set; }
        public ICollection<MonitoredInstance> MonitoredInstances { get; set; }
    }
}
