using Dapper;
using Npgsql;
using QueueNinja.Domain.Entities;
using QueueNinja.Infrastructure.Repositories;

namespace QueueNinja.Application.Services
{
    public class HangfireMonitoringService
    {
        private readonly MonitoredInstanceRepository _instanceRepository;

        public HangfireMonitoringService(MonitoredInstanceRepository instanceRepository)
        {
            _instanceRepository = instanceRepository;
        }

        public async Task<List<JobDto>> GetUserJobs(int instanceId)
        {
            var instance = await _instanceRepository.GetInstanceById(instanceId);
            if (instance == null) throw new Exception("Instance not found");

            using var conn = new NpgsqlConnection(instance.ConnectionString);
            var query = @"
                SELECT j.Id, j.InvocationData, s.Name as State, j.CreatedAt 
                FROM HangFire.Job j
                LEFT JOIN HangFire.State s ON j.StateId = s.Id
                ORDER BY j.CreatedAt DESC
                LIMIT 50;
            ";

            var jobs = await conn.QueryAsync<JobDto>(query);
            return jobs.ToList();
        }
    }
}
