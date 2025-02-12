using Microsoft.AspNetCore.Identity;

namespace QueueNinja.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<UserTenant> UserTenants { get; set; }
    }
}
