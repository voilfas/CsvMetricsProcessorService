using CsvMetricsProcessorService.Domain.Entities;
using CsvMetricsProcessorService.Domain.ValueObjects;
using FluentAssertions;
using Value = CsvMetricsProcessorService.Domain.ValueObjects.Value;

namespace CsvMetricsProcessorService.Domain.Tests.Entities;

public class MetricsValueTests
{
    [Fact]
    public void Create_ShouldReturnSuccess_WhenValueIsValid()
    {
        // Arrange
        var fileName = FileName.Create("001.csv").Value!;
        var date = new DateTime(2022, 01, 01);
        var executionTime = ExecutionTime.Create(120).Value!;
        var value = Value.Create(43.8).Value!;
        var currentDay = new DateTime(2023, 01, 01);
        

        // Act
        var result = MetricsValue.Create(
            fileName,
            date,
            executionTime,
            value,
            currentDay);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Error.Should().BeNull();
    }
    
    [Theory]
    [InlineData("2023-01-01 12:00:01")]
    [InlineData("1999-01-01 00:00:00")]
    public void Create_ShouldReturnFailure_WhenDateIsInvalid(
        string dateString)
    {
        // Arrange
        var fileName = FileName.Create("001.csv").Value!;
        DateTime date = DateTime.Parse(dateString);
        var executionTime = ExecutionTime.Create(120).Value!;
        var value = Value.Create(43.8).Value!;
        var currentDay = new DateTime(2023, 01, 01, 12, 00, 00);
        

        // Act
        var result = MetricsValue.Create(
            fileName,
            date,
            executionTime,
            value,
            currentDay);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Error.Should().NotBeNull();
        result.Error.Code.Should().Be("MetricsValue.InvalidDate");
    }
}