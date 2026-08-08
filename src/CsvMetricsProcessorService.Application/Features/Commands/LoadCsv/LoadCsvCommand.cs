using CsvMetricsProcessorService.Domain.Results;
using MediatR;

namespace CsvMetricsProcessorService.Application.Features.Commands.LoadCsv;

public record LoadCsvCommand(
    string FileName,
    Stream CsvStream
    ) : IRequest<Result>;