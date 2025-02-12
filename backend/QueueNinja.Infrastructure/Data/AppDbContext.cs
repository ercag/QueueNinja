using Microsoft.EntityFrameworkCore;
using QueueNinja.Domain.Entities;

namespace QueueNinja.Infrastructure.Data;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<UserTenant> UserTenants { get; set; }
    public DbSet<MonitoredInstance> MonitoredInstance { get; set; }
}
