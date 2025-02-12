using System;
using System.Threading.Tasks;

namespace QueueNinja.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IMonitoredInstanceRepository MonitoredInstances { get; }
        IUserTenantRepository UserTenants { get; }
        ITenantRepository Tenants { get; }

        Task<int> SaveChangesAsync();
    }
}
