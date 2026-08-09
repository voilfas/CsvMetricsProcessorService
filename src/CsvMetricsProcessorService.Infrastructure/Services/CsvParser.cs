using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvMetricsProcessorService.Application.Interfaces;
using CsvMetricsProcessorService.Domain.Entities;
using CsvMetricsProcessorService.Domain.Results;
using CsvMetricsProcessorService.Domain.ValueObjects;

namespace CsvMetricsProcessorService.Infrastructure.Services;

public class CsvParser : ICsvParser
{
    public async Task<Result<IReadOnlyList<MetricsValue>>> ParseAsync(
        Stream csvStream,
        FileName fileName,
        CancellationToken ct)
    {
        var records = new List<MetricsValue>(); 
        
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";",
            HasHeaderRecord = true,
            PrepareHeaderForMatch = args => args.Header.Trim()
        };

        using var reader = new StreamReader(csvStream);
        using var csv = new CsvReader(reader, config);
        
        try
        {
            await csv.ReadAsync();
            csv.ReadHeader();

            int rowCount = 0;
            
            var currentUtcTime = DateTime.UtcNow; 
            
            while (await csv.ReadAsync())
            {
                ct.ThrowIfCancellationRequested();
                rowCount++;
                
                if (rowCount > 10000)
                {
                    return Result<IReadOnlyList<MetricsValue>>.Failure(
                        new Error("Csv.TooManyRows", "Файл содержит более 10 000 строк."));
                }
                
                var rawDate = csv.GetField<string>("Date");
                var rawExecutionTime = csv.GetField<double>("ExecutionTime");
                var rawValue = csv.GetField<double>("Value");
                
                if (string.IsNullOrWhiteSpace(rawDate))
                {
                    return Result<IReadOnlyList<MetricsValue>>.Failure(
                        new Error("Csv.MissingValue", $"Ошибка в строке {rowCount}: Отсутствует значение Date."));
                }
                
                string expectedDateFormat = "yyyy'-'MM'-'dd'T'HH'-'mm'-'ss'.'fffffff'Z'";
                
                if (!DateTime.TryParseExact(rawDate, expectedDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime parsedDate))
                {
                    if (!DateTime.TryParse(rawDate, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out parsedDate))
                    {
                        return Result<IReadOnlyList<MetricsValue>>.Failure(
                            new Error("Csv.InvalidDateFormat", $"Ошибка в строке {rowCount}: Неверный формат даты '{rawDate}'."));
                    }
                }
                
                parsedDate = DateTime.SpecifyKind(parsedDate, DateTimeKind.Utc);
                
                var executionTimeResult = ExecutionTime.Create(rawExecutionTime);
                if (executionTimeResult.IsFailure)
                    return Result<IReadOnlyList<MetricsValue>>.Failure(executionTimeResult.Error!);

                var valueResult = Value.Create(rawValue);
                if (valueResult.IsFailure)
                    return Result<IReadOnlyList<MetricsValue>>.Failure(valueResult.Error!);
                
                var metricValueResult = MetricsValue.Create(
                    fileName,
                    parsedDate,
                    executionTimeResult.Value!,
                    valueResult.Value!,
                    currentUtcTime);

                if (metricValueResult.IsFailure)
                    return Result<IReadOnlyList<MetricsValue>>.Failure(metricValueResult.Error!);
                
                records.Add(metricValueResult.Value!);
            }
            
            if (rowCount == 0)
            {
                return Result<IReadOnlyList<MetricsValue>>.Failure(
                    new Error("Csv.EmptyFile", "Файл не содержит строк с данными."));
            }
            
            return Result<IReadOnlyList<MetricsValue>>.Success(records);
        }
        catch (Exception ex)
        {
            return Result<IReadOnlyList<MetricsValue>>.Failure(
                new Error("Csv.ParsingError", $"Критическая ошибка при чтении файла: {ex.Message}"));
        }
    }
}