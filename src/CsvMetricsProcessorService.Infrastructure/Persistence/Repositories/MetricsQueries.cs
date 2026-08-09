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
                AND (@MinDate::timestamp with time zone IS NULL OR min_date >= @MinDate::timestamp with time zone)
                AND (@MaxDate::timestamp with time zone IS NULL OR min_date <= @MaxDate::timestamp with time zone)
                AND (@MinAvgValue::double precision IS NULL OR avg_value >= @MinAvgValue::double precision)
                AND (@MaxAvgValue::double precision IS NULL OR avg_value <= @MaxAvgValue::double precision)
                AND (@MinAvgExecutionTime::double precision IS NULL OR avg_execution_time >= @MinAvgExecutionTime::double precision)
                AND (@MaxAvgExecutionTime::double precision IS NULL OR avg_execution_time <= @MaxAvgExecutionTime::double precision)
            ORDER BY created_at DESC;
        ";

        /*var result = await connection.QueryAsync<MetricsResultDto>(
            new CommandDefinition(sql, filter, cancellationToken: ct));*/
        
        var result = await connection.QueryAsync<MetricsResultDto>(
            new CommandDefinition(sql, new 
            {
                FileName = filter.FileName,
                MinDate = filter.GetMinDate(),
                MaxDate = filter.GetMaxDate(),
                MinAvgValue = filter.GetMinAvgValue(),
                MaxAvgValue = filter.GetMaxAvgValue(),
                MinAvgExecutionTime = filter.GetMinAvgExecutionTime(),
                MaxAvgExecutionTime = filter.GetMaxAvgExecutionTime()
            }, cancellationToken: ct));
        
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