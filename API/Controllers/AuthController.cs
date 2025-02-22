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

        public AuthController(PythonScriptService pythonScriptService,StoreContext context,UserManager<User> userManager,TokenService tokenService)
        {
            _context = context;
            _userManager = userManager;
            _tokenService=tokenService;
            _pythonScriptService=pythonScriptService;
        }

  [HttpGet("recognize")]
        public async Task<IActionResult> RecognizeUser(string name)
        {
            // Pozivanje Python skripte sa imenom korisnika kao argumentom
            string result = await _pythonScriptService.RecognizeFaceAsync();

            if (result.Contains("Welcome"))
            {
                return Ok(new { message = result }); // Korisnik prepoznat
            }
            else
            {
                return NotFound(new { message = "Nepoznata osoba" });
            }
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login (LoginDTO login){
            var user = await _userManager.Users
        .Include(u => u.TeachingClasses)  // Uključivanje TeachingClasses
        .FirstOrDefaultAsync(u => u.Email == login.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user,login.Password)) return Unauthorized();

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
