using Microsoft.AspNetCore.Mvc;
using QueueNinja.Application.Interfaces;
using QueueNinja.Domain.Entities;

namespace QueueNinja.Api.Controllers
{
    [ApiController]
    [Route("api/instances")]
    public class MonitoredInstanceController : ControllerBase
    {
        private readonly IMonitoredInstanceService _service;

        public MonitoredInstanceController(IMonitoredInstanceService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var instances = await _service.GetAllInstances();
            return Ok(instances);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] MonitoredInstance instance)
        {
            var newInstance = await _service.AddInstance(instance);
            return CreatedAtAction(nameof(GetAll), new { id = newInstance.Id }, newInstance);
        }
    }
}
