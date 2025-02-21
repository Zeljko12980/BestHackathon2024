using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tesseract;

[Route("api/[controller]")]
[ApiController]
public class DocumentController : ControllerBase
{
    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile image)
    {
        // Proveri da li je slika uploadovana
        if (image == null || image.Length == 0)
        {
            return BadRequest("No image uploaded.");
        }

        // Sačuvaj uploadovanu sliku u privremenu lokaciju
        string filePath = Path.GetTempFileName();

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(stream);
        }

        // Prepoznaj tekst sa slike
        string recognizedText = RecognizeText(filePath);

        // Izvuci JMB koristeći regularni izraz
        string jmb = ExtractJMB(recognizedText);

        // Vraćanje prepoznatog teksta i JMB za debagovanje
        return Ok(new { JMB = jmb, RecognizedText = recognizedText });
    }

    private string RecognizeText(string imagePath)
    {
        // Inicijalizacija Tesseract engine-a sa jezicima za prepoznavanje
        using (var engine = new TesseractEngine(@"./tessdata", "bos+srp+srp_latn", EngineMode.Default))
        {
            using (var img = Pix.LoadFromFile(imagePath))
            {
                using (var page = engine.Process(img))
                {
                    return page.GetText();
                }
            }
        }
    }

    private string ExtractJMB(string text)
{
    // Uklanjanje nepotrebnih karaktera, uključujući praznine, nove linije, specijalne znakove
    text = Regex.Replace(text, @"\s+", ""); // Ukloni sve praznine, nove linije i tabove

    // Regularni izraz za pronalaženje 13-znamenkastog JMBG broja
    var regex = new Regex(@"(?<!\d)\d{13}(?!\d)");
    var match = regex.Match(text);

    // Ako postoji podudaranje, vraćamo ga
    return match.Success ? match.Value : null;
}

}
