using CsvMetricsProcessorService.Application.DTOs;
using CsvMetricsProcessorService.Domain.Results;
using MediatR;

namespace CsvMetricsProcessorService.Application.Features.Queries.GetLatestValues;

public record GetLatestValuesQuery(
    string FileName
    ) : IRequest<Result<IReadOnlyList<MetricsValueDto>>>;