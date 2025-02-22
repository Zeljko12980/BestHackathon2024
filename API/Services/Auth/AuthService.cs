
namespace API.Services.Auth
{
    public class AuthService:IAuthService
    {
        private readonly StoreContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;

        public AuthService(StoreContext context,UserManager<User> userManager,ITokenService tokenService)
        {
            _context = context;
            _userManager = userManager;
            _tokenService=tokenService;
            
        }

       public async Task<UserDTO?> Login(LoginDTO login)
{
    // Pronađi korisnika po e-mailu i uključi razrede koje predaje
    var user = await _userManager.Users
        .Include(u => u.TeachingClasses)
        .FirstOrDefaultAsync(u => u.Email == login.Email);

    // Proveri da li je korisnik pronađen i da li je lozinka tačna
    if (user == null || !await _userManager.CheckPasswordAsync(user, login.Password))
        return null;  // Vraćamo null umesto Unauthorized()

    // Dohvati korisničke uloge
    var roles = await _userManager.GetRolesAsync(user);

    // Kreiranje i vraćanje UserDTO objekta sa podacima o korisniku
    return new UserDTO
    {
        FirstName = user.FirstName,
        LastName = user.LastName,
        Email = user.Email!,
        Token = await _tokenService.GenerateToken(user),
        UserName = user.UserName!,
        JMBG = user.JMBG,
        Id = user.Id,
        Roles = roles.ToList(),
        SchoolClassId = user.SchoolClassId,
        SchoolClassName = user.SchoolClass?.Name,
        TeachingClasses = user.TeachingClasses.Select(tc => tc.Name).ToList(),
    };
}


    }
}