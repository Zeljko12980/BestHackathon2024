namespace API.Services.Api
{
    public interface IApiService
    {
        Task<User> PrepoznajLiceAsync();
        Task<string> GameOnePlayAsync(string userId);
    }
}