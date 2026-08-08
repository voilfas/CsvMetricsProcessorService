using CsvMetricsProcessorService.Domain.ValueObjects;

namespace CsvMetricsProcessorService.Application.DTOs;

public record MetricsValueDto(
    Guid Id,
    string FileName,
    DateTime Date,
    double ExecutionTime,
    double Value,
    DateTime CreatedAt
    );