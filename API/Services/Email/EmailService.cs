using System.Net;
using System.Net.Mail;
using API.Models;
using API.Services.Interfaces;
using Microsoft.Extensions.Options;
using API.Services.Interfaces;

namespace API.Services
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

    public async Task SendEmailWithPdfAsync(string userId, string toEmail, string subject, string message)
    {
        if (string.IsNullOrEmpty(toEmail))
            throw new ArgumentNullException(nameof(toEmail), "Recipient email address cannot be null or empty.");

        if (string.IsNullOrEmpty(subject))
            throw new ArgumentNullException(nameof(subject), "Email subject cannot be null or empty.");

        if (string.IsNullOrEmpty(message))
            throw new ArgumentNullException(nameof(message), "Email message cannot be null or empty.");

        // Generate the PDF
        byte[] pdfContent;
        try
        {
            pdfContent = await _pdfService.CreatePdfFromUserDataAsync(userId);
            if (pdfContent == null || pdfContent.Length == 0)
            {
                throw new Exception("Generated PDF content is empty.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"PDF generation error: {ex.Message}");
            throw new Exception("Error generating PDF", ex);
        }
        Console.WriteLine(pdfContent.ToString());

        // Send the email
        try
        {
            var mail = new MailMessage()
            {
                From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName),
                Subject = subject,
                Body = message,
                IsBodyHtml = true // Set to true if your message contains HTML
            };
            mail.To.Add(new MailAddress(toEmail));

            if (pdfContent != null && pdfContent.Length > 0)
            {
                var attachment = new Attachment(new MemoryStream(pdfContent), "UserReport.pdf", "application/pdf");
                mail.Attachments.Add(attachment);
            }

            using (var smtp = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port))
            {
                smtp.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
                smtp.EnableSsl = true;

                // Log the configuration before sending
                Console.WriteLine($"SMTP Server: {_smtpSettings.Server}, Port: {_smtpSettings.Port}");
                Console.WriteLine("Sending email...");
                await smtp.SendMailAsync(mail);
                Console.WriteLine("Email sent successfully.");
            }
        }
        catch (SmtpException ex)
        {
            // Log more details
            Console.WriteLine($"SMTP error: {ex.Message}");
            throw new Exception($"SMTP error: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            // General exception logging
            Console.WriteLine($"General error: {ex.Message}");
            throw new Exception($"General error: {ex.Message}", ex);
        }
    }
public async Task SendEmailWithPassportPdfAsync(string userId, string toEmail, string subject, string message)
        {
            if (string.IsNullOrEmpty(toEmail))
                throw new ArgumentNullException(nameof(toEmail), "Recipient email address cannot be null or empty.");

            if (string.IsNullOrEmpty(subject))
                throw new ArgumentNullException(nameof(subject), "Email subject cannot be null or empty.");

            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException(nameof(message), "Email message cannot be null or empty.");

            // Generate the passport PDF
            byte[] pdfContent;
            try
            {
                pdfContent = await _pdfService.CreatePassportPdfFromUserDataAsync(userId); // Ensure you have a method for passport PDFs
                if (pdfContent == null || pdfContent.Length == 0)
                {
                    throw new Exception("Generated passport PDF content is empty.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Passport PDF generation error: {ex.Message}");
                throw new Exception("Error generating passport PDF", ex);
            }

            // Send the email
            try
            {
                var mail = new MailMessage()
                {
                    From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true // Set to true if your message contains HTML
                };
                mail.To.Add(new MailAddress(toEmail));

                if (pdfContent != null && pdfContent.Length > 0)
                {
                    var attachment = new Attachment(new MemoryStream(pdfContent), "Passport.pdf", "application/pdf");
                    mail.Attachments.Add(attachment);
                }

                using (var smtp = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port))
                {
                    smtp.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
                    smtp.EnableSsl = true;

                    // Log the configuration before sending
                    Console.WriteLine($"SMTP Server: {_smtpSettings.Server}, Port: {_smtpSettings.Port}");
                    Console.WriteLine("Sending email...");
                    await smtp.SendMailAsync(mail);
                    Console.WriteLine("Email sent successfully.");
                }
            }
            catch (SmtpException ex)
            {
                // Log more details
                Console.WriteLine($"SMTP error: {ex.Message}");
                throw new Exception($"SMTP error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                // General exception logging
                Console.WriteLine($"General error: {ex.Message}");
                throw new Exception($"General error: {ex.Message}", ex);
            }
        }
     public async Task SendEmailWithDriverLicensePdfAsync(string userId, string toEmail, string subject, string message)
        {
            if (string.IsNullOrEmpty(toEmail))
                throw new ArgumentNullException(nameof(toEmail), "Recipient email address cannot be null or empty.");

            if (string.IsNullOrEmpty(subject))
                throw new ArgumentNullException(nameof(subject), "Email subject cannot be null or empty.");

            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException(nameof(message), "Email message cannot be null or empty.");

            // Generate the driver's license PDF
            byte[] pdfContent;
            try
            {
                pdfContent = await _pdfService.CreateDriverLicensePdfFromUserDataAsync(userId);
                if (pdfContent == null || pdfContent.Length == 0)
                {
                    throw new Exception("Generated driver's license PDF content is empty.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Driver's license PDF generation error: {ex.Message}");
                throw new Exception("Error generating driver's license PDF", ex);
            }

            // Send the email
            try
            {
                var mail = new MailMessage()
                {
                    From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true // Set to true if your message contains HTML
                };
                mail.To.Add(new MailAddress(toEmail));

                if (pdfContent != null && pdfContent.Length > 0)
                {
                    var attachment = new Attachment(new MemoryStream(pdfContent), "DriverLicense.pdf", "application/pdf");
                    mail.Attachments.Add(attachment);
                }

                using (var smtp = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port))
                {
                    smtp.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
                    smtp.EnableSsl = true;

                    // Log the configuration before sending
                    Console.WriteLine($"SMTP Server: {_smtpSettings.Server}, Port: {_smtpSettings.Port}");
                    Console.WriteLine("Sending email...");
                    await smtp.SendMailAsync(mail);
                    Console.WriteLine("Email sent successfully.");
                }
            }
            catch (SmtpException ex)
            {
                // Log more details
                Console.WriteLine($"SMTP error: {ex.Message}");
                throw new Exception($"SMTP error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                // General exception logging
                Console.WriteLine($"General error: {ex.Message}");
                throw new Exception($"General error: {ex.Message}", ex);
            }
        }
}

}