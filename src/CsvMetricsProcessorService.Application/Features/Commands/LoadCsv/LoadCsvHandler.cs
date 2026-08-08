using CsvMetricsProcessorService.Application.Interfaces;
using CsvMetricsProcessorService.Domain.Results;
using CsvMetricsProcessorService.Domain.Services;
using CsvMetricsProcessorService.Domain.ValueObjects;
using MediatR;

namespace CsvMetricsProcessorService.Application.Features.Commands.LoadCsv;

public class LoadCsvHandler : IRequestHandler<LoadCsvCommand, Result>
{
    private readonly ICsvParser _csvParser;
    private readonly IMetricsRepository _metricsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LoadCsvHandler(
        ICsvParser csvParser, 
        IMetricsRepository metricsRepository,
        IUnitOfWork unitOfWork)
    {
        _csvParser = csvParser;
        _metricsRepository = metricsRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(
        LoadCsvCommand request, 
        CancellationToken ct)
    {
        var fileNameResult = FileName.Create(request.FileName);
        if (fileNameResult.IsFailure)
            return Result.Failure(fileNameResult.Error!);
        
        var fileName = fileNameResult.Value!;
        
        var listMetricsValueResult = await _csvParser
            .ParseAsync(request.CsvStream, fileName, ct);

        if (listMetricsValueResult.IsFailure)
            return Result.Failure(listMetricsValueResult.Error!);
        
        var listMetrics = listMetricsValueResult.Value!.ToList();
        
        var metricsResultR = MetricsProcessing.Calculate(fileName,listMetrics);
        if (metricsResultR.IsFailure)
            return Result.Failure(metricsResultR.Error!);
            
        var metricsResult = metricsResultR.Value!;
            

        if (await _metricsRepository.ExistsAsync(fileName, ct))
            await _metricsRepository.DeleteByFileNameAsync(fileName, ct);
        
        _metricsRepository.AddMetricsResult(metricsResult);
            
        await _metricsRepository.AddMetricsValueAsync(listMetrics, ct);

        await _unitOfWork.SaveChangesAsync(ct);
        
        return Result.Success();
    }
}