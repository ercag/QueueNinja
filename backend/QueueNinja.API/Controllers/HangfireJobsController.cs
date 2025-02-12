using Microsoft.AspNetCore.Mvc;
using QueueNinja.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace QueueNinja.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/jobs")]
    public class HangfireJobsController : ControllerBase
    {
        private readonly IHangfireMonitoringService _monitoringService;

        public HangfireJobsController(IHangfireMonitoringService monitoringService)
        {
            _monitoringService = monitoringService;
        }

        [HttpGet("{instanceId}")]
        public async Task<IActionResult> GetJobs(int instanceId)
        {
            var jobs = await _monitoringService.GetUserJobs(instanceId);
            return Ok(jobs);
        }

        // 🔄 Retry Job
        [HttpPost("retry/{jobId}")]
        public async Task<IActionResult> RetryJob(int jobId, [FromQuery] int instanceId)
        {
            var success = await _monitoringService.RetryJob(instanceId, jobId);
            return success ? Ok("Job retried successfully.") : BadRequest("Job retry failed.");
        }

        // 🗑 Delete Job
        [HttpDelete("{jobId}")]
        public async Task<IActionResult> DeleteJob(int jobId, [FromQuery] int instanceId)
        {
            var success = await _monitoringService.DeleteJob(instanceId, jobId);
            return success ? Ok("Job deleted successfully.") : BadRequest("Job deletion failed.");
        }

        [HttpGet("{instanceId}/history/{jobId}")]
        public async Task<IActionResult> GetJobHistory(int instanceId, int jobId)
        {
            var history = await _monitoringService.GetJobHistory(instanceId, jobId);
            return history != null ? Ok(history) : NotFound("No history found.");
        }

    }
}
