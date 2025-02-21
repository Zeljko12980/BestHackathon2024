using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly StoreContext _context;
        private readonly UserManager<User> _userManager;
        private readonly TokenService _tokenService;

        public AuthController(StoreContext context,UserManager<User> userManager,TokenService tokenService)
        {
            _context = context;
            _userManager = userManager;
            _tokenService=tokenService;
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login (LoginDTO login){
            var user = await _userManager.FindByEmailAsync(login.Email);
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
                Roles = roles.ToList(),
                AdresaPrebivalista = user.AdresaPrebivalista,
                OpstinaPrebivalista = user.OpstinaPrebivalista,
                Id =  user.Id,
            };

        }
        [HttpPost("register")]
        public async Task<ActionResult> Register (RegisterDTO registerDTO){

            DateOnly datumRodjenja;
            if (!DateOnly.TryParseExact(registerDTO.DatumRodjenja, "yyyy-MM-dd", out datumRodjenja))
            {
                ModelState.AddModelError("DatumRodjenja", "DatumRodjenja mora biti u formatu 'yyyy-MM-dd'.");
                return ValidationProblem();
            }

            var user =new User 
            {   
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                UserName = registerDTO.UserName,
                Email = registerDTO.Email,
                JMBG = registerDTO.JMBG,
                Pol = registerDTO.Pol,
                AdresaPrebivalista = registerDTO.AdresaPrebivalista,
                OpstinaPrebivalista = registerDTO.OpstinaPrebivalista,
                DatumRodjenja = datumRodjenja,
            };
            
            var result=await _userManager.CreateAsync(user,registerDTO.Password);
            await _userManager.AddToRoleAsync(user, "Member");
            if (!result.Succeeded){
                
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }     
                return ValidationProblem();
            }
            return StatusCode(201);
            
        }
       [Authorize]
        [HttpGet("currentUser")]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            // Proveri da li je identitet korisnika validan
            var userName = User.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
            {
                return Unauthorized(); // Ako korisnik nije autentifikovan
            }

            // Pokušaj da pronađeš korisnika po korisničkom imenu
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return NotFound(); // Ako korisnik ne postoji
            }

            // Kreiraj DTO sa podacima korisnika
            var userDTO = new UserDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                Token = await _tokenService.GenerateToken(user),
                UserName = user.UserName!,
                JMBG = user.JMBG,
                Id = user.Id,
            };

            return Ok(userDTO); // Vrati uspešan odgovor sa DTO-om
        }


        [Authorize]
        [HttpPut("edit")]
        public async Task<ActionResult<User>> EditUser([FromBody] UpdateUserDTO updateUserDTO)
        {
             var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

              if (userId == null)
            {
                return Unauthorized(new { message = "User is not authenticated." });
            }


            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

             if (user.Email != updateUserDTO.Email)
            {
                var existingUser = await _userManager.FindByEmailAsync(updateUserDTO.Email);
                if (existingUser != null)
                {
                    return Conflict(new { message = "Email already in use." });
                }
                user.Email = updateUserDTO.Email;
            }
         
            user.FirstName = updateUserDTO.FirstName;
            user.LastName = updateUserDTO.LastName;
            user.UserName = updateUserDTO.UserName;
            user.Email = updateUserDTO.Email;
            user.AdresaPrebivalista = updateUserDTO.AdresaPrebivalista;
            user.OpstinaPrebivalista = updateUserDTO.OpstinaPrebivalista;

            var result = await _userManager.UpdateAsync(user);
            
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return ValidationProblem();
            }
            
            return user;
        }



    }
}