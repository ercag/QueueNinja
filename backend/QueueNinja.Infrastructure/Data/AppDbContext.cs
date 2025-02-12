using Microsoft.EntityFrameworkCore;
using QueueNinja.Domain.Entities;

namespace QueueNinja.Infrastructure.Data;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<MonitoredInstance>? MonitoredInstances { get; }
}
