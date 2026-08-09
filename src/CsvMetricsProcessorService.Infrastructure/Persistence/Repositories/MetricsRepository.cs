using CsvMetricsProcessorService.Application.Interfaces;
using CsvMetricsProcessorService.Domain.Entities;
using CsvMetricsProcessorService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CsvMetricsProcessorService.Infrastructure.Persistence.Repositories;

public class MetricsRepository : IMetricsRepository
{
    private readonly MetricsDbContext _dbContext;
    private const int BatchSize = 2000;

    public MetricsRepository(MetricsDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<bool> ExistsAsync(
        FileName fileName,
        CancellationToken ct)
    {
        return await _dbContext.MetricsValues.AnyAsync(f => f.FileName == fileName, ct);
    }

    public async Task DeleteByFileNameAsync(
        FileName fileName,
        CancellationToken ct)
    {
         await _dbContext.MetricsValues
            .Where(f => f.FileName == fileName)
            .ExecuteDeleteAsync(ct);
         
         await _dbContext.MetricsResults
             .Where(r => r.FileName == fileName)
             .ExecuteDeleteAsync(ct);
    }

    public async Task AddMetricsValueAsync(
        List<MetricsValue> metricsValue,
        CancellationToken ct)
    {
        _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

        try
        {
            foreach (var batch in metricsValue.Chunk(BatchSize))
            {
                ct.ThrowIfCancellationRequested();
                
                await _dbContext.MetricsValues.AddRangeAsync(batch, ct);
                
                await _dbContext.SaveChangesAsync(ct);

                _dbContext.ChangeTracker.Clear();
            }
        }
        finally
        {
            _dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
        }
    }

    public void AddMetricsResult(MetricsResult metricsResult)
    {
        _dbContext.MetricsResults.Add(metricsResult);
    }
}