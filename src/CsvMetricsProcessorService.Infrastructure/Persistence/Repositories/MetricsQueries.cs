using System.Data;
using CsvMetricsProcessorService.Application.DTOs;
using CsvMetricsProcessorService.Application.Interfaces;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace CsvMetricsProcessorService.Infrastructure.Persistence.Repositories;

public class MetricsQueries : IMetricsQueries
{
    private readonly MetricsDbContext _dbContext;
    
    public MetricsQueries(MetricsDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IReadOnlyList<MetricsResultDto>> GetFilteredResultsAsync(
        MetricsFilterDto filter,
        CancellationToken ct)
    {
        using IDbConnection connection = _dbContext.Database.GetDbConnection();

        // lang=text
        var sql = @"
            SELECT
                id as Id,
                file_name as FileName,
                delta_date as DeltaDate,
                min_date as MinDate,
                avg_execution_time as AvgExecutionTime,
                avg_value as AvgValue,
                median_value as MedianValue,
                max_value as MaxValue,
                min_value as MinValue,
                created_at as CreatedAt
            FROM metrics_results
            WHERE (@FileName IS NULL OR file_name = @FileName)         
            AND (@MinDate IS NULL OR min_date >= @MinDate)
            AND (@MaxDate IS NULL OR min_date <= @MaxDate)
            AND (@MinAvgValue IS NULL OR min_value >= @MinAvgValue)
            AND (@MaxAvgValue IS NULL OR max_value <= @MaxAvgValue)
            AND (@MinAvgExecutionTime IS NULL OR avg_execution_time >= @MinExecutionTime)
            ANd (@MaxAvgExecutionTime IS NULL OR avg_execution_time <= @MaxExecutionTime)
            ORDER BY created_at DESC
        ";

        var result = await connection.QueryAsync<MetricsResultDto>(
            new CommandDefinition(sql, filter, cancellationToken: ct));
        
        return result.ToList();
    }

    public async Task<IReadOnlyList<MetricsValueDto>> GetLatestValuesAsync(
        string fileName,
        CancellationToken ct)
    {
        using IDbConnection connection = _dbContext.Database.GetDbConnection();

        // lang=text
        var sql = @"
            SELECT
                id as Id,
                file_name as FileName,
                date as Date,
                execution_time as ExecutionTime,
                value as Value,
                created_at as CreatedAt             
            FROM metrics_values
            WHERE file_name = @FileName
            ORDER BY date DESC, file_name ASC  
            LIMIT 10
        ";

        var result = await connection.QueryAsync<MetricsValueDto>(
        new CommandDefinition(sql, new { FileName = fileName} ,cancellationToken: ct));
        
        return result.ToList();
    }
}