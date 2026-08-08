using FluentAssertions;
using Value = CsvMetricsProcessorService.Domain.ValueObjects.Value;

namespace CsvMetricsProcessorService.Domain.Tests.ValueObjects;

public class ValueTests
{
    [Fact]
    public void Create_ShouldReturnSuccess_WhenIndicatorIsValid()
    {
        // Arrange
        double indicator = 12.7f;

        // Act
        var result = Value.Create(indicator);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Indicator.Should().Be(indicator);
    }
    
    [Fact]
    public void Create_ShouldReturnFailure_WhenIndicatorIsNegative()
    {
        // Arrange
        double indicator = -12.7f;

        // Act
        var result = Value.Create(indicator);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error.Code.Should().Be("Value.Negative");
    }
    
    [Fact]
    public void Create_ShouldReturnFailure_WhenIndicatorIsNaN()
    {
        // Arrange
        double indicator = double.NaN;

        // Act
        var result = Value.Create(indicator);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error.Code.Should().Be("Value.InfinityNan");
    }
    
    [Fact]
    public void Create_ShouldReturnFailure_WhenIndicatorIsInfinity()
    {
        // Arrange
        double indicator = double.PositiveInfinity;

        // Act
        var result = Value.Create(indicator);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error.Code.Should().Be("Value.InfinityNan");
    }
}