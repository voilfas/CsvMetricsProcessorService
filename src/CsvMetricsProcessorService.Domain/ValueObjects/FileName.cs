using CsvMetricsProcessorService.Domain.Errors;
using CsvMetricsProcessorService.Domain.Results;

namespace CsvMetricsProcessorService.Domain.ValueObjects;

public record FileName
{
    private const int MAX_LENGTH_NAME = 255;
    public string Value { get; private set; }

    private FileName(string value)
    {
        Value = value;
    }

    public static Result<FileName> Create(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return Result<FileName>.Failure(DomainErrors.FileName.Empty);
        
        var trimmedFileName = fileName.Trim();

        if (trimmedFileName.Length > MAX_LENGTH_NAME)
            return Result<FileName>.Failure(DomainErrors.FileName.TooLong);
        
        var invalidChars = Path.GetInvalidFileNameChars();
        if (trimmedFileName.Any(s => invalidChars.Contains(s)))
            return Result<FileName>.Failure(DomainErrors.FileName.InvalidChars);
        
        if (!trimmedFileName.EndsWith(".csv"))
            return Result<FileName>.Failure(DomainErrors.FileName.InvalidFileType);
        
        return Result<FileName>.Success(new FileName(fileName));
    }
    
    public override string ToString() =>  Value;
}