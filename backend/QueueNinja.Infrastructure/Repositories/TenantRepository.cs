using Microsoft.EntityFrameworkCore;
using QueueNinja.Application.Interfaces;
using QueueNinja.Domain.Entities;
using QueueNinja.Infrastructure.Data;

namespace QueueNinja.Infrastructure.Repositories;

public class TenantRepository : ITenantRepository
{
    private AppDbContext _context;

    public TenantRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Tenant?> GetTenantByIdAsync(int tenantId)
    {
        return await _context.Tenants.SingleOrDefaultAsync(_ => _.Id == tenantId);
    }
}
