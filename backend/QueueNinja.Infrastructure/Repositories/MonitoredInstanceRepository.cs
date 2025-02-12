using Microsoft.EntityFrameworkCore;
using QueueNinja.Domain.Entities;
using QueueNinja.Infrastructure.Data;

namespace QueueNinja.Infrastructure.Repositories
{
    public class MonitoredInstanceRepository
    {
        private readonly AppDbContext _context;

        public MonitoredInstanceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<MonitoredInstance>> GetAllInstances()
        {
            return await _context.MonitoredInstances?.ToListAsync();
        }
        public MonitoredInstance GetInstanceById(int instanceId)
        {
            return _context.MonitoredInstances.SingleOrDefault(_ => _.Id == instanceId);
        }

        public async Task<MonitoredInstance> AddInstance(MonitoredInstance instance)
        {
            _context.MonitoredInstances.Add(instance);
            await _context.SaveChangesAsync();
            return instance;
        }
    }
}
