using Microsoft.EntityFrameworkCore;
using QueueNinja.Application.Services;
using QueueNinja.Infrastructure.Data;
using QueueNinja.Infrastructure.Repositories;

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

// Register dependencies
builder.Services.AddScoped<MonitoredInstanceRepository>();
builder.Services.AddScoped<MonitoredInstanceService>();
builder.Services.AddScoped<HangfireMonitoringService>();

var app = builder.Build();

app.UseCors("AllowFrontend");

app.MapControllers(); // Enable API endpoints

app.Run();
