using Microsoft.EntityFrameworkCore;
using QueueNinja.Application.Services;
using QueueNinja.Infrastructure.Data;
using QueueNinja.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add PostgreSQL Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register dependencies
builder.Services.AddScoped<MonitoredInstanceRepository>();
builder.Services.AddScoped<MonitoredInstanceService>();

var app = builder.Build();

app.MapControllers(); // Enable API endpoints

app.Run();
