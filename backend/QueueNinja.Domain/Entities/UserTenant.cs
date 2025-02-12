namespace QueueNinja.Domain.Entities
{
    public class UserTenant
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
    }
}
