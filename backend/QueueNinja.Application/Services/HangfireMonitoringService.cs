using Dapper;
using Npgsql;
using QueueNinja.Domain.DTOs;
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
            var instance = _instanceRepository.GetInstanceById(instanceId);
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

        public async Task<bool> RetryJob(int instanceId, int jobId)
        {
            var instance = _instanceRepository.GetInstanceById(instanceId);
            if (instance == null) return false;

            using var conn = new NpgsqlConnection(instance.ConnectionString);
            var query = "INSERT INTO HangFire.JobQueue (JobId, Queue) VALUES (@JobId, 'default')";
            var affectedRows = await conn.ExecuteAsync(query, new { JobId = jobId });

            return affectedRows > 0;
        }

        public async Task<bool> DeleteJob(int instanceId, int jobId)
        {
            var instance = _instanceRepository.GetInstanceById(instanceId);
            if (instance == null) return false;

            using var conn = new NpgsqlConnection(instance.ConnectionString);
            var query = "DELETE FROM HangFire.Job WHERE Id = @JobId";
            var affectedRows = await conn.ExecuteAsync(query, new { JobId = jobId });

            return affectedRows > 0;
        }

        public async Task<List<JobHistoryDto>?> GetJobHistory(int instanceId, int jobId)
        {
            var instance = _instanceRepository.GetInstanceById(instanceId);
            if (instance == null) return null;

            using var conn = new NpgsqlConnection(instance.ConnectionString);
            var query = @"
                SELECT s.Name AS State, s.CreatedAt, p.Value AS ErrorMessage
                FROM HangFire.State s
                LEFT JOIN HangFire.JobParameter p ON s.JobId = p.JobId AND p.Name = 'ExceptionDetails'
                WHERE s.JobId = @JobId
                ORDER BY s.CreatedAt DESC;
            ";

            var history = await conn.QueryAsync<JobHistoryDto>(query, new { JobId = jobId });
            return history.ToList();
        }
    }
}
