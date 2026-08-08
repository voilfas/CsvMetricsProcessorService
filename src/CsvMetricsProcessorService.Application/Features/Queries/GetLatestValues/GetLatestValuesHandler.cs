using CsvMetricsProcessorService.Application.DTOs;
using CsvMetricsProcessorService.Application.Interfaces;
using CsvMetricsProcessorService.Domain.Results;
using MediatR;

namespace CsvMetricsProcessorService.Application.Features.Queries.GetLatestValues;

public class GetLatestValuesHandler : IRequestHandler<GetLatestValuesQuery, Result<IReadOnlyList<MetricsValueDto>>>
{
    private readonly IMetricsQueries _metricsQueries;

    public GetLatestValuesHandler(IMetricsQueries metricsQueries)
    {
        _metricsQueries = metricsQueries;
    }
    
    public async Task<Result<IReadOnlyList<MetricsValueDto>>> Handle(
        GetLatestValuesQuery request,
        CancellationToken ct)
    {
        var result = await _metricsQueries.GetLatestValuesAsync(
            request.FileName,
            ct);
        
        return Result<IReadOnlyList<MetricsValueDto>>.Success(result);
    }
}