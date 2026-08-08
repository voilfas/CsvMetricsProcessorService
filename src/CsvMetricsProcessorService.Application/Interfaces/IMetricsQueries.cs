using CsvMetricsProcessorService.Application.DTOs;

namespace CsvMetricsProcessorService.Application.Interfaces;

public interface IMetricsQueries
{
    Task<IReadOnlyList<MetricsResultDto>> GetFilteredResultsAsync(MetricsFilterDto filter, CancellationToken ct);
    
    Task<IReadOnlyList<MetricsValueDto>> GetLatestValuesAsync(string fileName, CancellationToken ct);
}