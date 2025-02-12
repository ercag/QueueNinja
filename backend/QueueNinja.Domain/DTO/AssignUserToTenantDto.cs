namespace QueueNinja.Domain.Dto
{
    public class AssignUserToTenantDto
    {
        public string UserId { get; set; }
        public int TenantId { get; set; }
    }
}