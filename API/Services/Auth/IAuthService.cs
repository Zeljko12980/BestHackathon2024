namespace API.Services.Auth
{
    public interface IAuthService
    {
        Task<UserDTO> Login (LoginDTO login);
    }
}