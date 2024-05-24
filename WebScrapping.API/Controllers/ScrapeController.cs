using Microsoft.AspNetCore.Mvc;
using WebScrapping.Service.Services;

namespace WebScrapping.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ScrapeController:ControllerBase
{
    private readonly RequestService _requestService;
    public ScrapeController( RequestService requestService)
    {

        _requestService = requestService;

    }
    
    [HttpGet("fetch")]
    public async Task<IActionResult> Fetch(string url)
    {
        var result = await _requestService.SendRequestAsync(url);
        return Ok(result);
    }
    


    
}