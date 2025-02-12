using Microsoft.EntityFrameworkCore;

namespace QueueNinja.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) { }

    public DbSet<MonitoredInstance> MonitoredInstances { get; }
}
