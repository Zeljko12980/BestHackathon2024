using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly StoreContext _context;
        private readonly UserManager<User> _userManager;
        private readonly TokenService _tokenService;
        private readonly PythonScriptService _pythonScriptService;

        public AuthController(StoreContext context, UserManager<User> userManager, TokenService tokenService)
        {
            _context = context;
            _userManager = userManager;
            _tokenService = tokenService;
        }

       [HttpPost("login")]
public async Task<ActionResult<UserDTO>> Login(LoginDTO login)
{
    var user = await _userManager.Users
        .Include(u => u.TeachingClasses)  // Učitaj TeachingClasses zajedno sa korisnikom
        .FirstOrDefaultAsync(u => u.Email == login.Email);

    if (user == null || !await _userManager.CheckPasswordAsync(user, login.Password)) 
        return Unauthorized();

    var roles = await _userManager.GetRolesAsync(user);

    var userDto = new UserDTO
    {
        FirstName = user.FirstName,
        LastName = user.LastName,
        Email = user.Email!,
        Token = await _tokenService.GenerateToken(user),
        UserName = user.UserName!,
        JMBG = user.JMBG,
        Roles = roles.ToList(),
        SchoolClassId = user.SchoolClassId,
        SchoolClassName = user.SchoolClass?.Name, // Dodajemo ime razreda ako postoji
        TeachingClasses = user.TeachingClasses.Select(tc => tc.Name).ToList(), // Popunjavaj TeachingClasses ovde
        Id = user.Id,
    };

    return Ok(userDto);
}


        [Authorize]
        [HttpGet("currentUser")]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var userName = User.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
                return Unauthorized();

            var user = await _userManager.Users
        .Include(u => u.TeachingClasses)  // Dodajemo Include za TeachingClasses
        .FirstOrDefaultAsync(u => u.UserName == userName);
            if (user == null)
                return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

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
