namespace CsvMetricsProcessorService.Application.DTOs;

public record MetricsResultDto(
    Guid Id,
    string FileName,
    double DeltaDate,
    DateTime MinDate,
    double AvgExecutionTime,
    double AvgValue,
    double MedianValue,
    double MaxValue,
    double MinValue,
    DateTime CreatedAt
    );