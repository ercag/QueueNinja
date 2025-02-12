using Microsoft.EntityFrameworkCore;

namespace QueueNinja.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base() { }
}
