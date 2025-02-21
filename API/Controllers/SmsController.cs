using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using Tesseract;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SmsController : ControllerBase
    {
        private readonly TwilioService _twilioService;

        public SmsController(TwilioService twilioService)
        {
            _twilioService = twilioService;
        }

        [HttpPost("send")]
        public IActionResult SendSms([FromBody] SmsRequest request)
        {
            _twilioService.SendSms(request.To, request.Message);
            return Ok("Message sent successfully.");
        }

        
    }
}
