using Microsoft.AspNetCore.Mvc;
using QueueNinja.Application.Services;
using QueueNinja.Domain.DTOs;

namespace QueueNinja.Api.Controllers
{
    [ApiController]
    [Route("api/jobs")]
    public class HangfireJobsController : ControllerBase
    {
        private readonly HangfireMonitoringService _monitoringService;

        public HangfireJobsController(HangfireMonitoringService monitoringService)
        {
            _monitoringService = monitoringService;
        }

        [HttpGet("{instanceId}")]
        public async Task<IActionResult> GetJobs(int instanceId)
        {
            var jobs = await _monitoringService.GetUserJobs(instanceId);
            return Ok(jobs);
        }
    }
}
