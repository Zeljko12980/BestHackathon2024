namespace API.Controllers
{
    public class AuthController : BaseAPIController
    {
        private readonly IAuthService _authService;
        private readonly IApiService _apiService;

        public AuthController(IAuthService authService,IApiService apiService)
        {
            _authService = authService;
            _apiService=apiService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            var userDto = await _authService.Login(login);

            if (userDto == null)
                return Unauthorized("Neispravan email ili lozinka.");

            return Ok(userDto);
        }

        [HttpPost("prepoznaj-lice")]
        public async Task<IActionResult> PrepoznajLice()
        {
            var result = await _apiService.PrepoznajLiceAsync();

            if (result.Contains("Greška") || result.Contains("Izuzetak"))
            {
                return BadRequest(new { message = result });
            }

            return Ok(new { message = "Korisnik prepoznat", ime = result });
        }
    }
}
