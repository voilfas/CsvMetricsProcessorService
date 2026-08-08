using CsvMetricsProcessorService.Domain.Entities;
using CsvMetricsProcessorService.Domain.Services;
using CsvMetricsProcessorService.Domain.ValueObjects;
using FluentAssertions;
using Value = CsvMetricsProcessorService.Domain.ValueObjects.Value;

namespace CsvMetricsProcessorService.Domain.Tests.Services;

public class MetricsProcessingTests
{
    [Fact]
    public void Calculate_ShouldReturnSuccessWithCorrectMetrics_WhenDataCountIsOdd()
    {
        // Arrange
        var fileName = FileName.Create("data.csv").Value!;
        var baseDate = new DateTime(2026, 08, 08, 12, 00, 00);
        var currentDay = new DateTime(2026, 08, 08, 15, 00, 00);
        
        var listMetrics = new List<MetricsValue>
        {
            MetricsValue.Create(fileName, baseDate, ExecutionTime.Create(1.0).Value!, Value.Create(10.0).Value!, currentDay).Value!,
            MetricsValue.Create(fileName, baseDate.AddSeconds(10), ExecutionTime.Create(2.0).Value!, Value.Create(30.0).Value!, currentDay).Value!,
            MetricsValue.Create(fileName, baseDate.AddSeconds(30), ExecutionTime.Create(3.0).Value!, Value.Create(20.0).Value!, currentDay).Value!
        };

        // Act
        var result = MetricsProcessing.Calculate(fileName, listMetrics);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Error.Should().BeNull();

        var metrics = result.Value!;
        metrics.FileName.Should().Be(fileName);
        metrics.MinDate.Should().Be(baseDate);
        metrics.DeltaDate.Should().Be(30.0);
        metrics.AvgExecutionTime.Should().Be(2.0); 
        metrics.AvgValue.Should().Be(20.0);
        metrics.MinValue.Should().Be(10.0);
        metrics.MaxValue.Should().Be(30.0);
        metrics.MedianValue.Should().Be(20.0);
    }

    [Fact]
    public void Calculate_ShouldReturnSuccessWithCorrectMedian_WhenDataCountIsEven()
    {
        // Arrange
        var fileName = FileName.Create("data.csv").Value!;
        var baseDate = new DateTime(2026, 08, 08, 12, 00, 00);
        var currentDay = new DateTime(2026, 08, 08, 15, 00, 00);
        
        var listMetrics = new List<MetricsValue>
        {
            MetricsValue.Create(fileName, baseDate, ExecutionTime.Create(1.0).Value!, Value.Create(40.0).Value!, currentDay).Value!,
            MetricsValue.Create(fileName, baseDate, ExecutionTime.Create(1.0).Value!, Value.Create(10.0).Value!, currentDay).Value!,
            MetricsValue.Create(fileName, baseDate, ExecutionTime.Create(1.0).Value!, Value.Create(30.0).Value!, currentDay).Value!,
            MetricsValue.Create(fileName, baseDate, ExecutionTime.Create(1.0).Value!, Value.Create(20.0).Value!, currentDay).Value!
        };

        // Act
        var result = MetricsProcessing.Calculate(fileName, listMetrics);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.MedianValue.Should().Be(25.0);
    }

    [Theory]
    [InlineData(null)]
    public void Calculate_ShouldReturnFailure_WhenListMetricsIsNullOrEmpty(List<MetricsValue>? invalidList)
    {
        // Arrange
        var fileName = FileName.Create("data.csv").Value!;
        var emptyList = new List<MetricsValue>();

        // Act
        var resultNull = MetricsProcessing.Calculate(fileName, invalidList);
        var resultEmpty = MetricsProcessing.Calculate(fileName, emptyList);

        // Assert
        resultNull.IsSuccess.Should().BeFalse();
        resultNull.Value.Should().BeNull();
        resultNull.Error.Should().NotBeNull();
        resultNull.Error!.Code.Should().Be("MetricsValues.EmptyList");

        resultEmpty.IsSuccess.Should().BeFalse();
        resultEmpty.Value.Should().BeNull();
        resultEmpty.Error.Should().NotBeNull();
        resultEmpty.Error!.Code.Should().Be("MetricsValues.EmptyList");
    }
}