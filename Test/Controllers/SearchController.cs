using Microsoft.AspNetCore.Mvc;
using Test.Models;
using TestTask;
namespace Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;
        private readonly ILogger<SearchController> _logger;
        public SearchController(ISearchService searchService, ILogger<SearchController> logger)
        {
            _searchService = searchService;
            _logger = logger;
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] SearchRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                _logger.LogWarning("Received null search request.");
                return BadRequest("Search request cannot be null.");
            }
            var response = await _searchService.SearchAsync(request, cancellationToken);

            _logger.LogInformation("Search completed successfully.");
            return Ok(response);
        }

        [HttpGet("ping/providers")]
        public async Task<IActionResult> PingProviders(CancellationToken cancellationToken)
        {
            var response = await _searchService.IsAvailableAsync(cancellationToken);
            return Ok(response);
        }

        [HttpGet("ping/api")]
        public IActionResult PingApi()
        {
            return Ok("API is available.");
        }
    }
}