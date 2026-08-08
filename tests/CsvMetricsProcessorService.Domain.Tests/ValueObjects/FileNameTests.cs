using CsvMetricsProcessorService.Domain.ValueObjects;
using FluentAssertions;

namespace CsvMetricsProcessorService.Domain.Tests.ValueObjects;

public class FileNameTests
{
    [Fact]
    public void Create_ShouldReturnSuccess_WhenFileNameIsValid()
    {
        // Arrange
        string fileName = "001.csv";

        // Act
        var result = FileName.Create(fileName);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Value.Should().Be(fileName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_ShouldReturnFailure_WhenFileNameIsNullOrWhitespace(
        string? fileName)
    {
        // Arrange

        // Act
        var result = FileName.Create(fileName!);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Error.Should().NotBeNull();
    }
    
    [Theory]
    [InlineData(" 001.csv")]
    [InlineData("001.csv ")]
    [InlineData("   001.csv    ")]
    public void Create_ShouldReturnSuccess_WhenValidFileNameWithWhitespace(
        string fileName)
    {
        // Arrange

        // Act
        var result = FileName.Create(fileName);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Value.Should().Be("001.csv");
    }
    
    [Fact]
    public void Create_ShouldReturnFailure_WhenFileNameTooLong()
    {
        // Arrange
        string fileName = $"{new string('q', 252)}.csv";

        // Act
        var result = FileName.Create(fileName);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Error.Should().NotBeNull();
        result.Error.Code.Should().Be("MetricsValue.MaxFileName");
    }
    
    [Theory]
    [InlineData("/001.csv")]
    [InlineData("\\001.csv")]
    [InlineData(":001.csv")]
    [InlineData("*001.csv")]
    [InlineData("?001.csv")]
    [InlineData("\"001.csv")]
    [InlineData("<001.csv")]
    [InlineData(">001.csv")]
    [InlineData("|001.csv")]
    public void Create_ShouldReturnFailure_WhenFileNameContainsInvalidFileNameChars(
        string fileName)
    {
        // Arrange

        // Act
        var result = FileName.Create(fileName);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Error.Should().NotBeNull();
        result.Error.Code.Should().Be("MetricsValue.InvalidChars");
    }
    
    [Fact]
    public void Create_ShouldReturnFailure_WhenFileNameInvalidEndsWith()
    {
        // Arrange
        string fileName = "001.csc";

        // Act
        var result = FileName.Create(fileName);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Error.Should().NotBeNull();
        result.Error.Code.Should().Be("MetricsValue.InvalidFileType");
    }
}