using Microsoft.EntityFrameworkCore;
using QueueNinja.Application.Services;
using QueueNinja.Application.Interfaces;
using QueueNinja.Infrastructure.Data;
using QueueNinja.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using QueueNinja.Domain.Entities;


var builder = WebApplication.CreateBuilder(args);

// 🔹 Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") // Allow frontend
                  .AllowAnyMethod() // Allow all HTTP methods (GET, POST, DELETE, etc.)
                  .AllowAnyHeader(); // Allow all headers
        });
});

builder.Services.AddControllers();

// Add PostgreSQL Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔹 Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Register dependencies
builder.Services.AddScoped<IMonitoredInstanceService, MonitoredInstanceService>();
builder.Services.AddScoped<IHangfireMonitoringService, HangfireMonitoringService>();
builder.Services.AddScoped<IUserTenantService, UserTenantService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // 🔹 Register Unit of Work


builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
