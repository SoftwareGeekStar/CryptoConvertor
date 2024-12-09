using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using CryptoConvertor.Services.Interfaces;

namespace CryptoConvertor.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CryptoController : ControllerBase
{

    private readonly ICryptoService _cryptoService;

    private readonly ILogger<CryptoController> _logger;

    public CryptoController(ICryptoService cryptoService)
    {
        _cryptoService = cryptoService;
    }

    [HttpGet("{cryptoSymbol}")]
    public async Task<IActionResult> GetCryptoQuote(string cryptoSymbol)
    {
        if(string.IsNullOrWhiteSpace(cryptoSymbol))
        {
            return BadRequest("Cryptocurrency symbol is required.");
        }

        try
        {
            var quote = await _cryptoService.GetCryptoQuotesAsync(cryptoSymbol);
            return Ok(quote);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
