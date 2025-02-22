namespace API.Services.Api
{
    public interface IApiService
    {
        Task<User> PrepoznajLiceAsync();
        Task<string> GameOnePlayAsync(string userId);
        Task<string> GameQuizzPlayAsync(string userId);

        Task<string> GameLeftPlayAsync(string userId);
    }
}