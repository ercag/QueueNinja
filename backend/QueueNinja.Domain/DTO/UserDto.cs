namespace QueueNinja.Domain.Dto
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public List<TenantDto> Tenants { get; set; }
    }

}