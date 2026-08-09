using CsvMetricsProcessorService.Application.DTOs;
using CsvMetricsProcessorService.Application.Interfaces;
using CsvMetricsProcessorService.Domain.Results;
using CsvMetricsProcessorService.Domain.ValueObjects;
using MediatR;

namespace CsvMetricsProcessorService.Application.Features.Queries.GetLatestValues;

public class GetLatestValuesHandler : IRequestHandler<GetLatestValuesQuery, Result<IReadOnlyList<MetricsValueDto>>>
{
    private readonly IMetricsQueries _metricsQueries;
    private readonly IMetricsRepository _metricsRepository;
    
    public GetLatestValuesHandler(
        IMetricsQueries metricsQueries,
        IMetricsRepository metricsRepository)
    {
        _metricsQueries = metricsQueries;
        _metricsRepository = metricsRepository;
    }
    
    public async Task<Result<IReadOnlyList<MetricsValueDto>>> Handle(
        GetLatestValuesQuery request,
        CancellationToken ct)
    {
        var fileNameResult = FileName.Create(request.FileName);
        if (fileNameResult.IsFailure)
            return Result<IReadOnlyList<MetricsValueDto>>.Failure(fileNameResult.Error!);
        
        var fileName = fileNameResult.Value!;
        
        if (!await _metricsRepository.ExistsAsync(fileName, ct))
            return Result<IReadOnlyList<MetricsValueDto>>.Failure(
                new Error("FileName.NotFound", "File not found"));
        
        
        var result = await _metricsQueries.GetLatestValuesAsync(
            request.FileName,
            ct);
        
        return Result<IReadOnlyList<MetricsValueDto>>.Success(result);
    }
}