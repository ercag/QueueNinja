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

        // ✅ Get Recent 50 Jobs from Hangfire
        public async Task<List<JobDto>> GetUserJobs(int instanceId)
        {
            var instance = await _instanceRepository.GetInstanceById(instanceId);
            if (instance == null) throw new Exception("Instance not found");

            using var conn = new NpgsqlConnection(instance.ConnectionString);
            var query = @"
                SELECT j.id, j.invocationdata, 
                    (SELECT statename FROM hangfire.state s WHERE s.jobid = j.id ORDER BY createdat DESC LIMIT 1) AS state, 
                    j.createdat 
                FROM hangfire.job j
                ORDER BY j.createdat DESC
                LIMIT 50;
            ";

            var jobs = await conn.QueryAsync<JobDto>(query);
            return jobs.ToList();
        }

        // ✅ Retry Job by Enqueuing It Again
        public async Task<bool> RetryJob(int instanceId, int jobId)
        {
            var instance = await _instanceRepository.GetInstanceById(instanceId);
            if (instance == null) return false;

            using var conn = new NpgsqlConnection(instance.ConnectionString);

            var query = @"
                INSERT INTO hangfire.state (jobid, statename, createdat)
                VALUES (@JobId, 'Enqueued', NOW());
            ";

            var affectedRows = await conn.ExecuteAsync(query, new { JobId = jobId });

            return affectedRows > 0;
        }

        // ✅ Delete Job from Hangfire
        public async Task<bool> DeleteJob(int instanceId, int jobId)
        {
            var instance = await _instanceRepository.GetInstanceById(instanceId);
            if (instance == null) return false;

            using var conn = new NpgsqlConnection(instance.ConnectionString);

            var query = @"
                DELETE FROM hangfire.state WHERE jobid = @JobId;
                DELETE FROM hangfire.jobparameter WHERE jobid = @JobId;
                DELETE FROM hangfire.job WHERE id = @JobId;
            ";

            var affectedRows = await conn.ExecuteAsync(query, new { JobId = jobId });

            return affectedRows > 0;
        }

        // ✅ Get Job Execution History
        public async Task<List<JobHistoryDto>?> GetJobHistory(int instanceId, int jobId)
        {
            var instance = await _instanceRepository.GetInstanceById(instanceId);
            if (instance == null) return null;

            using var conn = new NpgsqlConnection(instance.ConnectionString);
            var query = @"
                SELECT s.statename AS state, s.createdat, 
                       COALESCE(p.value, '') AS errormessage
                FROM hangfire.state s
                LEFT JOIN hangfire.jobparameter p ON s.jobid = p.jobid AND p.name = 'ExceptionDetails'
                WHERE s.jobid = @JobId
                ORDER BY s.createdat DESC;
            ";

            var history = await conn.QueryAsync<JobHistoryDto>(query, new { JobId = jobId });
            return history.ToList();
        }
    }
}
