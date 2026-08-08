using CsvMetricsProcessorService.Domain.Entities;
using CsvMetricsProcessorService.Domain.Results;
using CsvMetricsProcessorService.Domain.ValueObjects;

namespace CsvMetricsProcessorService.Application.Interfaces;

public interface ICsvParser
{
    Task<Result<IReadOnlyList<MetricsValue>>> ParseAsync(Stream csvStream, FileName fileName, CancellationToken ct);
}