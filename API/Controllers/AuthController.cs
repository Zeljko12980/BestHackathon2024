namespace API.Controllers
{
    public class AuthController : BaseAPIController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            var userDto = await _authService.Login(login);

            if (userDto == null)
                return Unauthorized("Neispravan email ili lozinka.");

            return Ok(userDto);
        }
    }
}
