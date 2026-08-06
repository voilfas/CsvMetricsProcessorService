using CsvMetricsProcessorService.Domain.Errors;
using CsvMetricsProcessorService.Domain.Results;

namespace CsvMetricsProcessorService.Domain.ValueObjects;

public record Value
{
    public double Indicator { get; private set; }

    private Value(double indicator)
    {
        Indicator = indicator;
    }

    public static Result<Value> Create(double indicator)
    {
        if (double.IsInfinity(indicator) || double.IsNaN(indicator))
            return Result<Value>.Failure(
                DomainErrors.Value.InfinityNan);

        if (indicator < 0)
            return Result<Value>.Failure(
                DomainErrors.Value.Negative);

        return Result<Value>.Success(new Value(indicator));
    }
}