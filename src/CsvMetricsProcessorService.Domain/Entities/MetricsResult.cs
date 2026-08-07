using CsvMetricsProcessorService.Domain.Common;
using CsvMetricsProcessorService.Domain.Errors;
using CsvMetricsProcessorService.Domain.Results;
using CsvMetricsProcessorService.Domain.ValueObjects;

namespace CsvMetricsProcessorService.Domain.Entities;

public class MetricsResult : BaseEntity
{
    public FileName? FileName { get; private set; }
    public double DeltaDate { get; private set; }
    public DateTime MinDate { get; private set; }
    public double AvgExecutionTime { get; private set; }
    public double AvgValue { get; private set; }
    public double MedianValue { get; private set; }
    public double MaxValue { get; private set; }
    public double MinValue { get; private set; }

    private MetricsResult(
        FileName fileName,
        double deltaDate,
        DateTime minDate,
        double avgExecutionTime,
        double avgValue,
        double medianValue,
        double maxValue,
        double minValue)
    {
        FileName = fileName;
        DeltaDate = deltaDate;
        MinDate = minDate;
        AvgExecutionTime = avgExecutionTime;
        AvgValue = avgValue;
        MedianValue = medianValue;
        MaxValue = maxValue;
        MinValue = minValue;
    }

    public static Result<MetricsResult> Create(
        FileName? fileName,
        double deltaDate,
        DateTime minDate,
        double avgExecutionTime,
        double avgValue,
        double medianValue,
        double maxValue,
        double minValue)
    {
        if (fileName is null)
            return Result<MetricsResult>.Failure(DomainErrors.MetricsResult.NullFileName);
        
        if (deltaDate < 0)
            return Result<MetricsResult>.Failure(DomainErrors.MetricsResult.InvalidDeltaDate);

        if (avgExecutionTime < 0)
            return Result<MetricsResult>.Failure(DomainErrors.MetricsResult.InvalidAvgExecutionTime);
        
        if (double.IsNaN(deltaDate) || double.IsInfinity(deltaDate) ||
            double.IsNaN(avgExecutionTime) || double.IsInfinity(avgExecutionTime) ||
            double.IsNaN(avgValue) || double.IsInfinity(avgValue) ||
            double.IsNaN(medianValue) || double.IsInfinity(medianValue) ||
            double.IsNaN(maxValue) || double.IsInfinity(maxValue) ||
            double.IsNaN(minValue) || double.IsInfinity(minValue))
        {
            return Result<MetricsResult>.Failure(DomainErrors.MetricsResult.InvalidNumericValues);
        }
        
        if (minValue > maxValue)
            return Result<MetricsResult>.Failure(DomainErrors.MetricsResult.MinGreaterThanMax);

        return Result<MetricsResult>.Success(
            new MetricsResult(
                fileName,
                deltaDate,
                minDate,
                avgExecutionTime,
                avgValue,
                medianValue,
                maxValue,
                minValue));
    }
}