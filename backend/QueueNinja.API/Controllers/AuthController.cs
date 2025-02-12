using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QueueNinja.Domain.Entities;
using QueueNinja.Domain.Dto;
using QueueNinja.Application.Services;

namespace QueueNinja.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserTenantService _tenantService;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
        UserTenantService tenantService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tenantService = tenantService;
        }

        // ✅ Register a New User
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // 🔹 Assign User to Default Tenant
            var tenantId = 1; // Default tenant ID (change this logic later)
            await _tenantService.AssignUserToTenantAsync(user.Id, tenantId);

            return Ok("User registered successfully and assigned to a tenant.");
        }


        // ✅ Login User
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (!result.Succeeded)
                return Unauthorized("Invalid email or password.");

            return Ok("Login successful.");
        }
    }
}
