using CsvMetricsProcessorService.Domain.ValueObjects;
using FluentAssertions;

namespace CsvMetricsProcessorService.Domain.Tests.ValueObjects;

public class ExecutionTimeTests
{
    [Fact]
    public void Create_ShouldReturnSuccess_WhenExecutionTimeSpanIsValid()
    {
        // Arrange
        TimeSpan executionTime = TimeSpan.FromHours(1);

        // Act
        var result = ExecutionTime.Create(executionTime);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.TotalSeconds.Should().Be(3600);
    }
    
    [Fact]
    public void Create_ShouldReturnSuccess_WhenExecutionTimeSecondsIsValid()
    {
        // Arrange
        double executionTime = 125.5;

        // Act
        var result = ExecutionTime.Create(executionTime);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.TotalSeconds.Should().Be(125.5);
    }
    
    [Fact]
    public void Create_ShouldReturnFailure_WhenExecutionTimeSpanLessZero()
    {
        // Arrange
        TimeSpan executionTime = TimeSpan.FromHours(-1);

        // Act
        var result = ExecutionTime.Create(executionTime);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
    }
    
    [Fact]
    public void Create_ShouldReturnFailure_WhenExecutionTimeSpanMoreHour()
    {
        // Arrange
        TimeSpan executionTime = TimeSpan.FromHours(1.1);

        // Act
        var result = ExecutionTime.Create(executionTime);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
    }
    
    [Theory]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NaN)]
    public void Create_ShouldReturnFailure_WhenExecutionTimeSecondsInfinityOrNan(
        double executionTime)
    {
        // Arrange

        // Act
        var result = ExecutionTime.Create(executionTime);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
    }
    
    [Fact]
    public void Create_ShouldReturnFailure_WhenExecutionTimeSecondsLessZero()
    {
        // Arrange
        double executionTime = -125.5;

        // Act
        var result = ExecutionTime.Create(executionTime);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
    }
}