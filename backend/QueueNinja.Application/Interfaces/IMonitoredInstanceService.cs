using QueueNinja.Domain.Entities;

namespace QueueNinja.Application.Interfaces
{
    public interface IMonitoredInstanceService
    {
        Task<List<MonitoredInstance>> GetAllInstances();

        Task<MonitoredInstance> AddInstance(MonitoredInstance instance);
    }
}
