namespace API.Services.Score
{
  public class ScoreService : IScoreService
{
    private readonly UserManager<User> _userManager;

    public ScoreService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> IncreaseScoreAsync(string userId, int scoreIncrease)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return false;
        }

        user.Score += scoreIncrease;
        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded;
    }
}
}