using CsvMetricsProcessorService.Application.Interfaces;

namespace CsvMetricsProcessorService.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly MetricsDbContext _dbContext;
    
    public UnitOfWork(MetricsDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task SaveChangesAsync(CancellationToken ct)
    {
        await _dbContext.SaveChangesAsync(ct);
    }
}