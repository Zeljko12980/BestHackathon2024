namespace API.Services.Token
{
    public interface ITokenService
    {
         Task<string> GenerateToken(User user);
    }
}