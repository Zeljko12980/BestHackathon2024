namespace API.Controllers
{
    public class AuthController : BaseAPIController
    {
        private readonly IAuthService _authService;
        private readonly IApiService _apiService;

     private static readonly HttpClient client = new HttpClient();
        private readonly UserManager<User> _userManager;
        public AuthController(IAuthService authService,IApiService apiService,UserManager<User> userManager)
        {
            _authService = authService;
            _apiService=apiService;
            _userManager = userManager;
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
    var user = await _apiService.PrepoznajLiceAsync();
    if (user == null)
    {
        return NotFound("Korisnik nije pronađen.");
    }

    return Ok(user);
}


    }
}
