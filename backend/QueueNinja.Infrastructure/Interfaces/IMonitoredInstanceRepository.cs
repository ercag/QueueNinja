using QueueNinja.Domain.Entities;
using System.Threading.Tasks;

namespace QueueNinja.Application.Interfaces
{
    public interface IMonitoredInstanceRepository
    {
        Task<MonitoredInstance?> GetInstanceById(int instanceId);
        Task<List<MonitoredInstance>> GetAllAsync();
    }
}
