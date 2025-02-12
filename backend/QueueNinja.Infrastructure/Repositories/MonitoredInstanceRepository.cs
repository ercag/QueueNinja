using Microsoft.EntityFrameworkCore;
using QueueNinja.Application.Interfaces;
using QueueNinja.Domain.Entities;
using QueueNinja.Infrastructure.Data;

namespace QueueNinja.Infrastructure.Repositories
{
    public class MonitoredInstanceRepository : IMonitoredInstanceRepository
    {
        private readonly AppDbContext _context;

        public MonitoredInstanceRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<MonitoredInstance> GetInstanceById(int instanceId)
        {
            return _context.MonitoredInstance.SingleOrDefaultAsync(_ => _.Id == instanceId);
        }

        public async Task<MonitoredInstance> AddInstance(MonitoredInstance instance)
        {
            _context.MonitoredInstance.Add(instance);
            await _context.SaveChangesAsync();
            return instance;
        }

        public async Task<List<MonitoredInstance>> GetAllAsync()
        {
            return await _context.MonitoredInstance.ToListAsync();
        }
    }
}
