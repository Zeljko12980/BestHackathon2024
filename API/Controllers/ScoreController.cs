using API.Services.Score;

public class ScoreController : BaseAPIController
{
    private readonly IScoreService _scoreService;

    public ScoreController(IScoreService scoreService)
    {
        _scoreService = scoreService;
    }

    [HttpPut("increase-score/{userId}/{scoreIncrease}")]
    public async Task<IActionResult> IncreaseScore(string userId, int scoreIncrease)
    {
        bool success = await _scoreService.IncreaseScoreAsync(userId, scoreIncrease);

        if (!success)
        {
            return NotFound("Korisnik nije pronađen ili ažuriranje nije uspelo.");
        }

        return Ok("Score je uspešno povećan");
    }
}