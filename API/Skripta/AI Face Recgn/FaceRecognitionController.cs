using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using API.Data;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;
<<<<<<< HEAD
using Microsoft.EntityFrameworkCore;

=======
>>>>>>> 690e2ac79a7e145e7089cec8aaa861c86445192a


namespace API.Skripta.AI_Face_Recgn
{
    [Route("api/[controller]")]
    [ApiController]
    public class FaceRecognitionController : ControllerBase
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly UserManager<User> _userManager;
        private readonly StoreContext _context;

        public FaceRecognitionController(StoreContext context,UserManager<User> userManager)
        {
            _context = context;
            _userManager=userManager;
        }

        // Akcija za prepoznavanje lica
<<<<<<< HEAD
       [HttpPost("prepoznaj-lice")]
public async Task<IActionResult> PrepoznajLice()
{
    try
    {
        // URL Flask servera
        var url = "http://localhost:5000/prepoznaj-lice";

        // Slanje POST zahteva
        var response = await client.PostAsync(url, null);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            var cleanedResult = result.Trim('"');  // Uklanja navodnike sa početka i kraja
            cleanedResult = Regex.Replace(cleanedResult, "[^a-zA-Z]", "");  // Filtrira samo slova

            // Pronalazi korisnika po korisničkom imenu
          var user = await _userManager.Users
    .FirstOrDefaultAsync(u => u.UserName.ToLower() == cleanedResult.ToLower());



            if (user != null)
            {
                return Ok(user); // Vraća korisničke podatke
            }
            else
            {
                return NotFound("Korisnik nije pronađen.");
            }
        }
        else
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            return BadRequest($"Greška: {errorMessage}");
        }
    }
    catch (HttpRequestException httpEx)
    {
        // Specifična greška u vezi sa HTTP zahtevima
        return StatusCode(500, $"Greška pri slanju HTTP zahteva: {httpEx.Message}");
    }
    catch (Exception ex)
    {
        // Generalna greška
        return StatusCode(500, $"Greška pri povezivanju sa serverom: {ex.Message}");
    }
}

        }
    }
=======
        [HttpPost("prepoznaj-lice")]
        public async Task<IActionResult> PrepoznajLice()
        {
            try
            {
                // URL Flask servera
                var url = "http://localhost:5000/prepoznaj-lice";

                // Slanje POST zahteva
                var response = await client.PostAsync(url, null);

                if (response.IsSuccessStatusCode)
                {
                   var result = await response.Content.ReadAsStringAsync();
// Ako je prepoznavanje uspešno
var cleanedResult = result.Trim('"');  // Uklanja navodnike sa početka i kraja

// Regex za filtriranje samo slova
cleanedResult = Regex.Replace(cleanedResult, "[^a-zA-Z]", "");

// Sada koristi cleanedResult
var user = await _userManager.FindByNameAsync(cleanedResult);
return Ok(user); 
// Vraća rezultat sa servera (ime osobe ili greška)
                }
                else
                {
                    // Ako server vrati grešku
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return BadRequest($"Greška: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greška pri povezivanju sa serverom: {ex.Message}");
            }
        }
    }
}
>>>>>>> 690e2ac79a7e145e7089cec8aaa861c86445192a
