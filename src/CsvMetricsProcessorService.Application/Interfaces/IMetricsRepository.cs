using CsvMetricsProcessorService.Domain.Entities;
using CsvMetricsProcessorService.Domain.Results;
using CsvMetricsProcessorService.Domain.ValueObjects;

namespace CsvMetricsProcessorService.Application.Interfaces;

public interface IMetricsRepository
{
    Task<bool> ExistsAsync(FileName fileName, CancellationToken ct);
    
    Task DeleteByFileNameAsync(FileName fileName, CancellationToken ct);
    
    Task AddMetricsValueAsync(List<MetricsValue> metricsValue, CancellationToken ct);
    
    void AddMetricsResult(MetricsResult metricsResult);
}