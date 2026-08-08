using CsvMetricsProcessorService.Application.DTOs;
using CsvMetricsProcessorService.Domain.Results;
using MediatR;

namespace CsvMetricsProcessorService.Application.Features.Queries.GetMetricsResultFiltered;

public record GetMetricsResultFilteredQuery(
    MetricsFilterDto Filter
    ) : IRequest<Result<IReadOnlyList<MetricsResultDto>>>;