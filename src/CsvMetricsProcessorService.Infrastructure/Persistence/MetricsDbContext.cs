using CsvMetricsProcessorService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CsvMetricsProcessorService.Infrastructure.Persistence;

public class MetricsDbContext : DbContext
{
    public DbSet<MetricsValue> MetricsValues => Set<MetricsValue>();
    public DbSet<MetricsResult> MetricsResults => Set<MetricsResult>();

    public MetricsDbContext(DbContextOptions<MetricsDbContext> options) 
        : base (options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MetricsDbContext).Assembly);
    }
}