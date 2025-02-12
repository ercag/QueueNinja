using Microsoft.EntityFrameworkCore;
using QueueNinja.Application.Interfaces;
using QueueNinja.Domain.Entities;
using QueueNinja.Infrastructure.Data;

namespace QueueNinja.Application.Services
{
    public class UserTenantService : IUserTenantService
    {
        private readonly AppDbContext _context;

        public UserTenantService(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Assign User to a Tenant
        public async Task AssignUserToTenantAsync(string userId, int tenantId)
        {
            var existingAssignment = await _context.UserTenants
                .FirstOrDefaultAsync(ut => ut.UserId == userId && ut.TenantId == tenantId);

            if (existingAssignment == null)
            {
                var userTenant = new UserTenant
                {
                    UserId = userId,
                    TenantId = tenantId
                };

                _context.UserTenants.Add(userTenant);
                await _context.SaveChangesAsync();
            }
        }
    }
}
