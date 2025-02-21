using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController:ControllerBase
    {
         private readonly IEmailService _emailService;

    public EmailController(IEmailService emailService)
    {
        _emailService = emailService;
    }

            [HttpPost("send-passport-pdf")]
        public async Task<IActionResult> SendPassportPdfEmailAsync([FromQuery] string userId, [FromQuery] string toEmail, [FromQuery] string subject, [FromQuery] string message)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(toEmail) || string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(message))
            {
                return BadRequest("Invalid input parameters.");
            }

            try
            {
                await _emailService.SendEmailWithPassportPdfAsync(userId, toEmail, subject, message);
                return Ok("Email with passport PDF sent successfully.");
            }
            catch (Exception ex)
            {
                // Log exception if using a logging framework
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


 [HttpPost("send-pdf")]
        public async Task<IActionResult> SendPdfEmail([FromQuery] string userId, [FromQuery] string toEmail, [FromQuery] string subject, [FromQuery] string message)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(toEmail) || string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(message))
            {
                return BadRequest("Invalid input parameters.");
            }

            try
            {
                await _emailService.SendEmailWithPdfAsync(userId, toEmail, subject, message);
                return Ok("Email with PDF sent successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

          [HttpPost("send-driver-license-email")]
        public async Task<IActionResult> SendDriverLicenseEmail([FromQuery] string userId, [FromQuery] string toEmail, [FromQuery] string subject, [FromQuery] string message)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(toEmail) || string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(message))
            {
                return BadRequest("Missing required parameters.");
            }

            try
            {
                await _emailService.SendEmailWithDriverLicensePdfAsync(userId, toEmail, subject, message);
                return Ok("Email sent successfully.");
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
   
    }
}