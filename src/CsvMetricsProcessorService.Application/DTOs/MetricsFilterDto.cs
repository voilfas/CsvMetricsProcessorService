using CsvMetricsProcessorService.Domain.ValueObjects;

namespace CsvMetricsProcessorService.Application.DTOs;

public record MetricsFilterDto(
    string? FileName = null,
    string? DateRange = null,
    string? ValueRange = null,
    string? ExecutionTimeRange = null
)
{
    public DateTime? MinDate => ParseDateRange(DateRange).From;
    public DateTime? MaxDate => ParseDateRange(DateRange).To;
    
    public double? MinAvgValue => ParseDoubleRange(ValueRange).From;
    public double? MaxAvgValue => ParseDoubleRange(ValueRange).To;
    
    public double? MinAvgExecutionTime => ParseDoubleRange(ExecutionTimeRange).From;
    public double? MaxAvgExecutionTime => ParseDoubleRange(ExecutionTimeRange).To;

    private (double? From, double? To) ParseDoubleRange(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return (null, null);

        var parts = value.Split(['-', ' ', '/'], StringSplitOptions.RemoveEmptyEntries);

        double? from = parts.Length > 0 && double.TryParse(parts[0].Replace(',', '.'), out var tryFrom) ? tryFrom : null;
        double? to = parts.Length > 1 && double.TryParse(parts[1].Replace(',', '.'), out var tryTo) ? tryTo : null;
        
        return (from, to);
    }

    private (DateTime? From, DateTime? To) ParseDateRange(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return (null, null);

        var parts = value.Split(['-', ' ', '/'], StringSplitOptions.RemoveEmptyEntries);

        DateTime? from = parts.Length > 0 && DateTime.TryParse(parts[0], out var tryFrom) ? tryFrom : null;
        DateTime? to = parts.Length > 1 && DateTime.TryParse(parts[1], out var tryTo) ? tryTo : null;
        
        return (from, to);
    }
}