using CsvMetricsProcessorService.Domain.Errors;
using CsvMetricsProcessorService.Domain.Results;

namespace CsvMetricsProcessorService.Domain.ValueObjects;

public record ExecutionTime
{
    public TimeSpan Time { get; private set; }
    public double TotalSeconds => Time.TotalSeconds;
    
    private ExecutionTime(TimeSpan time)
    {
        Time = time;
    }

    public static Result<ExecutionTime> Create(TimeSpan time)
    {
        if (time < TimeSpan.Zero)
            return Result<ExecutionTime>.Failure(
                DomainErrors.ExecutionTime.Negative);

        if (time > TimeSpan.FromHours(1))
            return Result<ExecutionTime>.Failure(
                DomainErrors.ExecutionTime.TooLong);

        return Result<ExecutionTime>.Success(new ExecutionTime(time));
    }

    public static Result<ExecutionTime> Create(double seconds)
    {
        if (double.IsInfinity(seconds) || double.IsNaN(seconds))
            return Result<ExecutionTime>.Failure(
                DomainErrors.ExecutionTime.InfinityNan);

        if (seconds < 0)
            return Result<ExecutionTime>.Failure(
                DomainErrors.ExecutionTime.Negative);

        var timeSpan = TimeSpan.FromSeconds(seconds);
        return Create(timeSpan);
    }
}