namespace API.Services.Score
{
    public interface IScoreService
    {
        Task<bool> IncreaseScoreAsync(string userId, int scoreIncrease);
    }
}