using CsvMetricsProcessorService.Domain.Entities;
using CsvMetricsProcessorService.Domain.ValueObjects;
using FluentAssertions;

namespace CsvMetricsProcessorService.Domain.Tests.Entities;

public class MetricsResultTests
{
    [Fact]
    public void Create_ShouldReturnSuccess_WhenDataIsValid()
    {
        // Arrange
        var fileName = FileName.Create("001.csv").Value;
        var deltaDate = 1563.4;
        var minDate = new DateTime(2005, 01, 01);
        var avgExecutionTime = 15.5;
        var avgValue = 63.00;
        var medianValue = 14.00;
        var maxValue = 512.5;
        var minValue = 234.5;

        // Act
        var result = MetricsResult.Create(
            fileName,
            deltaDate,
            minDate,
            avgExecutionTime,
            avgValue,
            medianValue,
            maxValue,
            minValue);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Error.Should().BeNull();
    }
    
    [Fact]
    public void Create_ShouldReturnFailure_WhenDataMaxValueLessThanMinValue()
    {
        // Arrange
        var fileName = FileName.Create("001.csv").Value;
        var deltaDate = 1563.4;
        var minDate = new DateTime(2005, 01, 01);
        var avgExecutionTime = 15.5;
        var avgValue = 63.00;
        var medianValue = 14.00;
        var maxValue = 512.5;
        var minValue = 2434.5;

        // Act
        var result = MetricsResult.Create(
            fileName,
            deltaDate,
            minDate,
            avgExecutionTime,
            avgValue,
            medianValue,
            maxValue,
            minValue);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Error.Should().NotBeNull();
        result.Error.Code.Should().Be("MetricsResult.MinGreaterThanMax");
    }
    
    [Fact]
    public void Create_ShouldReturnFailure_WhenDeltaDateLessZero()
    {
        // Arrange
        var fileName = FileName.Create("001.csv").Value;
        var deltaDate = -1;
        var minDate = new DateTime(2005, 01, 01);
        var avgExecutionTime = 15.5;
        var avgValue = 63.00;
        var medianValue = 14.00;
        var maxValue = 512.5;
        var minValue = 434.5;

        // Act
        var result = MetricsResult.Create(
            fileName,
            deltaDate,
            minDate,
            avgExecutionTime,
            avgValue,
            medianValue,
            maxValue,
            minValue);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Error.Should().NotBeNull();
        result.Error.Code.Should().Be("MetricsResult.InvalidDeltaDate");
    }
    
    [Fact]
    public void Create_ShouldReturnFailure_WhenAvgExecutionTimeLessZero()
    {
        // Arrange
        var fileName = FileName.Create("001.csv").Value;
        var deltaDate = 24;
        var minDate = new DateTime(2005, 01, 01);
        var avgExecutionTime = -15.5;
        var avgValue = 63.00;
        var medianValue = 14.00;
        var maxValue = 512.5;
        var minValue = 434.5;

        // Act
        var result = MetricsResult.Create(
            fileName,
            deltaDate,
            minDate,
            avgExecutionTime,
            avgValue,
            medianValue,
            maxValue,
            minValue);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Error.Should().NotBeNull();
        result.Error.Code.Should().Be("MetricsResult.InvalidAvgExecutionTime");
    }
}