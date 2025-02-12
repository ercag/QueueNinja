using QueueNinja.Application.Interfaces;
using QueueNinja.Domain.Entities;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace QueueNinja.Application.Services
{
    public class MonitoredInstanceService : IMonitoredInstanceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MonitoredInstanceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<MonitoredInstance> AddInstance(MonitoredInstance instance)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MonitoredInstance>> GetAllInstances()
        {
            return await _unitOfWork.MonitoredInstances.GetAllAsync();
        }

        public async Task<MonitoredInstance?> GetInstanceById(int instanceId)
        {
            return await _unitOfWork.MonitoredInstances.GetInstanceById(instanceId);
        }
    }
}
