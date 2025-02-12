using QueueNinja.Domain.Entities;
using QueueNinja.Infrastructure.Repositories;

namespace QueueNinja.Application.Services
{
    public class MonitoredInstanceService
    {
        private readonly MonitoredInstanceRepository _repository;

        public MonitoredInstanceService(MonitoredInstanceRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<MonitoredInstance>> GetAllInstances()
        {
            return await _repository.GetAllInstances();
        }

        public async Task<MonitoredInstance> AddInstance(MonitoredInstance instance)
        {
            return await _repository.AddInstance(instance);
        }
    }
}
