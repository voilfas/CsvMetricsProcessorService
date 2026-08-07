using CsvMetricsProcessorService.Domain.Common;
using CsvMetricsProcessorService.Domain.Errors;
using CsvMetricsProcessorService.Domain.Results;
using CsvMetricsProcessorService.Domain.ValueObjects;

namespace CsvMetricsProcessorService.Domain.Entities;

public class MetricsValue : BaseEntity
{
    private static readonly DateTime MinDate = new DateTime(2000, 1, 1);
    public FileName FileName { get; private set; }
    public DateTime Date { get; private set; }
    public ExecutionTime ExecutionTime { get; private set; }
    public Value Value { get; private set; }

    #pragma warning disable CS8618
    private MetricsValue() { }
    #pragma warning restore CS8618

    private MetricsValue(
        FileName fileName,
        DateTime date,
        ExecutionTime executionTime,
        Value value)
    {
        FileName = fileName;
        Date = date;
        ExecutionTime = executionTime;
        Value = value;
    }

    public static Result<MetricsValue> Create(
        FileName fileName,
        DateTime date,
        ExecutionTime executionTime,
        Value value,
        DateTime currentDate)
    {
        if (date > currentDate || date < MinDate)
            return Result<MetricsValue>.Failure(
                DomainErrors.MetricsValue.InvalidDate);
        
        return Result<MetricsValue>.Success(
            new MetricsValue(fileName, date, executionTime, value));
    }
}