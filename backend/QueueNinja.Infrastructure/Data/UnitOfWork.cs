using QueueNinja.Application.Interfaces;
using QueueNinja.Infrastructure.Repositories;

namespace QueueNinja.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IMonitoredInstanceRepository MonitoredInstances { get; }
        public IUserTenantRepository UserTenants { get; }
        public ITenantRepository Tenants { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            MonitoredInstances = new MonitoredInstanceRepository(_context);
            UserTenants = new UserTenantRepository(_context);
            Tenants = new TenantRepository(_context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
