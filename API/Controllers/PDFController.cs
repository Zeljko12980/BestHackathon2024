using Microsoft.AspNetCore.Mvc;
using API.Services.Interfaces;
using API.Services;
namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PDFController:ControllerBase
    {
        private readonly IPDFService _pdfService;

        public PDFController(IPDFService pdfService)
        {
            _pdfService=pdfService;
        }

     

        [HttpGet("generate/{userId}")]
        public async Task<IActionResult> GeneratePdf(string userId)
        {
            try
            {
                var pdfBytes = await _pdfService.CreatePdfFromUserDataAsync(userId);

                return File(pdfBytes, "application/pdf", "prijavnica.pdf");
               // return Ok(pdfBytes);
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}