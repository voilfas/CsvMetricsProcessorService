using CsvMetricsProcessorService.Domain.Entities;
using CsvMetricsProcessorService.Domain.Errors;
using CsvMetricsProcessorService.Domain.Results;
using CsvMetricsProcessorService.Domain.ValueObjects;

namespace CsvMetricsProcessorService.Domain.Services;

public class MetricsProcessing
{
    public static Result<MetricsResult> Calculate(
        FileName fileName, 
        IReadOnlyList<MetricsValue>? listMetrics)
    {
        if (listMetrics is null || listMetrics.Count == 0)
            return Result<MetricsResult>.Failure(DomainErrors.MetricsValue.EmptyOrNullMetrics);

        var sortedValues = listMetrics
            .Select(e => e.Value.Indicator)
            .OrderBy(i => i)
            .ToList();
        
        int count = sortedValues.Count;
        var minValue = sortedValues[0];
        var maxValue = sortedValues[^1];

        double medianValue = count % 2 != 0
            ? sortedValues[count / 2]
            : (sortedValues[count / 2] + sortedValues[count / 2 - 1]) / 2.0;

        DateTime minDate = listMetrics[0].Date;
        DateTime maxDate = listMetrics[0].Date;

        double totalValue = 0.0;
        double totalExecutionTime = 0.0;
        
        foreach (var metric in listMetrics)
        {
            if (metric.Date < minDate) minDate = metric.Date;
            if (metric.Date > maxDate) maxDate = metric.Date;
            
            totalValue +=  metric.Value.Indicator;
            totalExecutionTime += metric.ExecutionTime.TotalSeconds;
        }

        var deltaDate = (maxDate - minDate).TotalSeconds;
        var avgExecutionTime = totalExecutionTime / count;
        var avgValue = totalValue / count;

        var metricsResult = MetricsResult.Create(
            fileName,
            deltaDate,
            minDate,
            avgExecutionTime,
            avgValue,
            medianValue,
            maxValue,
            minValue);
        
        if (metricsResult.IsFailure)
            return Result<MetricsResult>.Failure(metricsResult.Error!);
        
        return Result<MetricsResult>.Success(metricsResult.Value!);
    }
}