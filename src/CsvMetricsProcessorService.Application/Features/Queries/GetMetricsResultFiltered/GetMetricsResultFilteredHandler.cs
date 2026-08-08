using CsvMetricsProcessorService.Application.DTOs;
using CsvMetricsProcessorService.Application.Interfaces;
using CsvMetricsProcessorService.Domain.Results;
using MediatR;

namespace CsvMetricsProcessorService.Application.Features.Queries.GetMetricsResultFiltered;

public class GetMetricsResultFilteredHandler : IRequestHandler<GetMetricsResultFilteredQuery, Result<IReadOnlyList<MetricsResultDto>>>
{
    private readonly IMetricsQueries _metricsQueries;
    
    public GetMetricsResultFilteredHandler(
        IMetricsQueries metricsQueries)
    {
        _metricsQueries = metricsQueries;
    }
    
    public async Task<Result<IReadOnlyList<MetricsResultDto>>> Handle(
        GetMetricsResultFilteredQuery request,
        CancellationToken ct)
    {
        var result =  await _metricsQueries.GetFilteredResultsAsync(
            request.Filter,
            ct);

        return Result<IReadOnlyList<MetricsResultDto>>.Success(result);
    }
}