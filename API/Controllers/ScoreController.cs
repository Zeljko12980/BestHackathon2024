using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScoreController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public ScoreController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpPut("increase-score/{userId}/{scoreIncrease}")]
        public async Task<IActionResult> IncreaseScore(string userId, int scoreIncrease)
        {
            // Pronađi korisnika prema userId
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound("Korisnik nije pronađen");
            }

            // Povećaj score
            user.Score += scoreIncrease;

            // Spasi promene u bazi pomoću UserManager
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest("Greška pri ažuriranju korisnika");
            }

            return Ok("Score je uspešno povećan");
        }
    }
}
