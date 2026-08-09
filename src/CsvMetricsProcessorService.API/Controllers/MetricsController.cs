using CsvMetricsProcessorService.Application.DTOs;
using CsvMetricsProcessorService.Application.Features.Commands.LoadCsv;
using CsvMetricsProcessorService.Application.Features.Queries.GetLatestValues;
using CsvMetricsProcessorService.Application.Features.Queries.GetMetricsResultFiltered;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CsvMetricsProcessorService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MetricsController : ControllerBase
{
    private readonly ISender _sender;
    
    public MetricsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LoadCsv(
        IFormFile file,
        CancellationToken ct)
    {
        if (file is null || file.Length == 0)
            return BadRequest("File not load or empty");

        string rawFileName = file.FileName;

        using var stream = file.OpenReadStream();

        var command = new LoadCsvCommand(rawFileName, stream);
        var result = await _sender.Send(command, ct);
        
        if (result.IsFailure)
            return BadRequest(new { error = result.Error!.Message, code = result.Error.Code });
        
        return Ok("File successfully loaded and save");
    }
    
    [HttpGet("results")]
    [ProducesResponseType(typeof(IReadOnlyList<MetricsResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetFilteredResults([FromQuery] MetricsFilterDto filter, CancellationToken ct)
    {
        // Отправляем запрос в твой GetMetricsResultFilteredHandler
        var query = new GetMetricsResultFilteredQuery(filter);
        var result = await _sender.Send(query, ct);

        if (result.IsFailure)
            return BadRequest(new { error = result.Error!.Message });

        return Ok(result.Value);
    }
    
    [HttpGet("values/latest")]
    [ProducesResponseType(typeof(IReadOnlyList<MetricsValueDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetLatestValues([FromQuery] string fileName, CancellationToken ct)
    {
        // Отправляем запрос в твой GetLatestValuesHandler
        var query = new GetLatestValuesQuery(fileName);
        var result = await _sender.Send(query, ct);

        if (result.IsFailure)
            return BadRequest(new { error = result.Error!.Message, code = result.Error.Code });

        return Ok(result.Value);
    }

}