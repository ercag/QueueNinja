using QueueNinja.Domain.Dto;

namespace QueueNinja.Application.Interfaces
{
    public interface IHangfireMonitoringService
    {

        // ✅ Get Recent 50 Jobs from Hangfire
        Task<List<JobDto>> GetUserJobs(int instanceId);

        // ✅ Retry Job by Enqueuing It Again
        Task<bool> RetryJob(int instanceId, int jobId);

        // ✅ Delete Job from Hangfire
        Task<bool> DeleteJob(int instanceId, int jobId);

        // ✅ Get Job Execution History
        Task<List<JobHistoryDto>?> GetJobHistory(int instanceId, int jobId);
    }
}
