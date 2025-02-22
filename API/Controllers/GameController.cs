namespace API.Controllers
{
    public class GameController:BaseAPIController
    {
        private readonly IApiService _apiService;

        public GameController(IApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpPost("start-game")]
    public async Task<IActionResult> StartGame([FromQuery] string userId)
    {
        try
        {
            var result = await _apiService.GameOnePlayAsync(userId);

            if (result.Contains("Greška"))
            {
                return BadRequest(result);  // Ako dođe do greške u Flask API-ju
            }
            return Ok(new { message = "Game started successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }
    }
}