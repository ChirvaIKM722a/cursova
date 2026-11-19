using Microsoft.EntityFrameworkCore;

namespace LightingScada.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<SensorSample> SensorSamples => Set<SensorSample>();
}
