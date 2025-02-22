

namespace API.Services.Email
{
  public class EmailService : IEmailService
{
    private readonly SmtpSettings _smtpSettings;
    private readonly IPDFService _pdfService;

    public EmailService(IOptions<SmtpSettings> smtpSettings, IPDFService pdfService)
    {
        _smtpSettings = smtpSettings.Value;
        _pdfService = pdfService;
    }

        public Task SendEmailWithDriverLicensePdfAsync(string userId, string toEmail, string subject, string message)
        {
            throw new NotImplementedException();
        }

        public Task SendEmailWithPassportPdfAsync(string userId, string toEmail, string subject, string message)
        {
            throw new NotImplementedException();
        }

        public Task SendEmailWithPdfAsync(string userId, string toEmail, string subject, string message)
        {
            throw new NotImplementedException();
        }
    }

}