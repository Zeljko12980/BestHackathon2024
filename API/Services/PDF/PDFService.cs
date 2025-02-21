using System;
using System.IO;
using System.Threading.Tasks;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Identity;
using API.Models;
using API.Services.Interfaces;
using System.Text;


public class PDFService : IPDFService
{
    private readonly UserManager<User> _userManager;

    public PDFService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<byte[]> CreatePassportPdfFromUserDataAsync(string userId)
{
    var user = await _userManager.FindByIdAsync(userId);
    if (user == null)
    {
        throw new Exception("User not found");
    }

    // Collect current date
    string todayDate = DateTime.Now.ToString("yyyy-MM-dd");

    // Collect user details
    string firstName = user.FirstName ?? "Ime";
    string lastName = user.LastName ?? "Prezime";
    string birthDate = user.DatumRodjenja.ToString("yyyy-MM-dd") ?? "Datum rođenja";
    string jmbg = user.JMBG ?? "JMBG";
    string address = user.AdresaPrebivalista ?? "Adresa prebivališta";
    string nationality =  "Nacionalnost";  // Assuming you have this field in your user model

    // Use StringBuilder to create HTML content
    var htmlBuilder = new StringBuilder();

    htmlBuilder.AppendLine(@"<!DOCTYPE html>
    <html lang='bs'>
    <head>
        <meta charset='UTF-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>ZAHTJEV ZA PASOŠ</title>
        <style>
            body { font-family: Arial, sans-serif; background-color: #f5f5f5; margin: 0; padding: 20px; }
            .form-container { max-width: 800px; margin: auto; padding: 20px; background-color: #ffffff; border: 1px solid #dddddd; border-radius: 8px; box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1); }
            h1, h2 { text-align: center; color: #333333; }
            h1 { margin-bottom: 20px; }
            h2 { margin: 20px 0; }
            .section-top, .section-middle, .section-main { margin-bottom: 20px; }
            .row { display: flex; justify-content: space-between; margin-bottom: 10px; }
            .label { flex: 1; margin-right: 10px; font-weight: bold; }
            .value { flex: 2; padding: 5px; border: 1px solid #cccccc; border-radius: 4px; background-color: #f5f5f5; }
            p { text-align: center; margin-bottom: 20px; font-style: italic; color: #666666; }
            .legal-section { margin-top: 30px; font-size: 14px; color: #555555; border-top: 1px solid #dddddd; padding-top: 20px; }
            .signature-box { border: 1px solid #000; width: 200px; height: 100px; margin: 20px auto; text-align: center; padding-top: 40px; }
        </style>
    </head>
    <body>
        <div class='form-container'>
            <h1>ZAHTJEV ZA IZDAVANJE PASOŠA</h1>
            <section class='section-top'>
                <div class='row'>
                    <span class='label'>Naziv organa:</span>
                    <span class='value'>eGRAD</span>
                </div>
                <div class='row'>
                    <span class='label'>Broj:</span>
                    <span class='value'>B23</span>
                </div>
                <div class='row'>
                    <span class='label'>Datum:</span>
                    <span class='value'>");

    htmlBuilder.AppendLine(todayDate);
    htmlBuilder.AppendLine(@"</span>
                </div>
            </section>
            <section class='section-middle'>
                <div class='row'>
                    <span class='label'>Ime:</span>
                    <span class='value'>");

    htmlBuilder.AppendLine(firstName);
    htmlBuilder.AppendLine(@"</span>
                </div>
                <div class='row'>
                    <span class='label'>Prezime:</span>
                    <span class='value'>");

    htmlBuilder.AppendLine(lastName);
    htmlBuilder.AppendLine(@"</span>
                </div>
                <div class='row'>
                    <span class='label'>Datum rođenja:</span>
                    <span class='value'>");

    htmlBuilder.AppendLine(birthDate);
    htmlBuilder.AppendLine(@"</span>
                </div>
                <div class='row'>
                    <span class='label'>JMBG:</span>
                    <span class='value'>");

    htmlBuilder.AppendLine(jmbg);
    htmlBuilder.AppendLine(@"</span>
                </div>
                <div class='row'>
                    <span class='label'>Adresa prebivališta:</span>
                    <span class='value'>");

    htmlBuilder.AppendLine(address);
    htmlBuilder.AppendLine(@"</span>
                </div>
                <div class='row'>
                    <span class='label'>Nacionalnost:</span>
                    <span class='value'>");

    htmlBuilder.AppendLine(nationality);
    htmlBuilder.AppendLine(@"</span>
                </div>
            </section>
            <section class='section-main'>
                <h2>PODACI O PASOŠU</h2>
                <div class='row'>
                    <span class='label'>Broj pasoša:</span>
                    <span class='value'>123456789</span> <!-- Example static value -->
                </div>
                <div class='row'>
                    <span class='label'>Datum izdavanja:</span>
                    <span class='value'>2024-08-30</span> <!-- Example static value -->
                </div>
                <div class='row'>
                    <span class='label'>Datum isteka:</span>
                    <span class='value'>2034-08-30</span> <!-- Example static value -->
                </div>
            </section>
            <section class='legal-section'>
                <h2>Zakonska regulativa</h2>
                <p>Prikupljanje, obrada, i čuvanje podataka u ovoj prijavnici vrši se u skladu sa Zakonom o zaštiti ličnih podataka Bosne i Hercegovine.</p>
            </section>
            <div class='signature-box'>
                <p>Potpis</p>
            </div>
        </div>
    </body>
    </html>");

    var htmlContent = htmlBuilder.ToString();

    using (var memoryStream = new MemoryStream())
    {
        using (var pdfWriter = new PdfWriter(memoryStream))
        {
            using (var pdfDocument = new PdfDocument(pdfWriter))
            {
                // Convert HTML to PDF using the memory stream
                HtmlConverter.ConvertToPdf(new MemoryStream(Encoding.UTF8.GetBytes(htmlContent)), pdfDocument);
            }
        }

        return memoryStream.ToArray();
    }
}

public async Task<byte[]> CreateDriverLicensePdfFromUserDataAsync(string userId)
{
    var user = await _userManager.FindByIdAsync(userId);
    if (user == null)
    {
        throw new Exception("User not found");
    }

    // Collect current date
    string todayDate = DateTime.Now.ToString("yyyy-MM-dd");

    // Collect user name and other details
    string firstName = user.FirstName ?? "Ime";
    string lastName = user.LastName ?? "Prezime";
    string birthDate = user.DatumRodjenja.ToString("yyyy-MM-dd") ?? "Datum rođenja";
    string jmbg = user.JMBG ?? "JMBG";
    string address = user.AdresaPrebivalista ?? "Adresa prebivališta";

    // Use StringBuilder to create HTML content
    var htmlBuilder = new StringBuilder();

    htmlBuilder.AppendLine(@"<!DOCTYPE html>
    <html lang='bs'>
    <head>
        <meta charset='UTF-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>ZAHTJEV ZA VOZAČKU DOZVOLU</title>
        <style>
            body { font-family: Arial, sans-serif; background-color: #f5f5f5; margin: 0; padding: 20px; }
            .form-container { max-width: 800px; margin: auto; padding: 20px; background-color: #ffffff; border: 1px solid #dddddd; border-radius: 8px; box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1); }
            h1, h2 { text-align: center; color: #333333; }
            h1 { margin-bottom: 20px; }
            h2 { margin: 20px 0; }
            .section-top, .section-middle, .section-main { margin-bottom: 20px; }
            .row { display: flex; justify-content: space-between; margin-bottom: 10px; }
            .label { flex: 1; margin-right: 10px; font-weight: bold; }
            .value { flex: 2; padding: 5px; border: 1px solid #cccccc; border-radius: 4px; background-color: #f5f5f5; }
            p { text-align: center; margin-bottom: 20px; font-style: italic; color: #666666; }
            .legal-section { margin-top: 30px; font-size: 14px; color: #555555; border-top: 1px solid #dddddd; padding-top: 20px; }
            .signature-box { border: 1px solid #000; width: 200px; height: 100px; margin: 20px auto; text-align: center; padding-top: 40px; }
        </style>
    </head>
    <body>
        <div class='form-container'>
            <h1>ZAHTJEV ZA IZDAVANJE, PRODUŽENJE VAŽENJA I ZAMJENU VOZAČKE DOZVOLE</h1>
            <section class='section-top'>
                <div class='row'>
                    <span class='label'>Naziv organa:</span>
                    <span class='value'>eGRAD</span>
                </div>
                <div class='row'>
                    <span class='label'>Broj:</span>
                    <span class='value'>B23</span>
                </div>
                <div class='row'>
                    <span class='label'>Datum:</span>
                    <span class='value'>");

    htmlBuilder.AppendLine(todayDate);
    htmlBuilder.AppendLine(@"</span>
                </div>
            </section>
            <section class='section-middle'>
                <div class='row'>
                    <span class='label'>Ime:</span>
                    <span class='value'>");

    htmlBuilder.AppendLine(firstName);
    htmlBuilder.AppendLine(@"</span>
                </div>
                <div class='row'>
                    <span class='label'>Prezime:</span>
                    <span class='value'>");

    htmlBuilder.AppendLine(lastName);
    htmlBuilder.AppendLine(@"</span>
                </div>
                <div class='row'>
                    <span class='label'>Datum rođenja:</span>
                    <span class='value'>");

    htmlBuilder.AppendLine(birthDate);
    htmlBuilder.AppendLine(@"</span>
                </div>
                <div class='row'>
                    <span class='label'>JMBG:</span>
                    <span class='value'>");

    htmlBuilder.AppendLine(jmbg);
    htmlBuilder.AppendLine(@"</span>
                </div>
                <div class='row'>
                    <span class='label'>Adresa prebivališta:</span>
                    <span class='value'>");

    htmlBuilder.AppendLine(address);
    htmlBuilder.AppendLine(@"</span>
                </div>
            </section>
            <section class='section-main'>
                <h2>PODACI O VOZAČKOJ DOZVOLI</h2>
                <div class='row'>
                    <span class='label'>Kategorija:</span>
                    <span class='value'>A, B, C</span>
                </div>
                <div class='row'>
                    <span class='label'>Datum položenog ispita:</span>
                    <span class='value'>2024-08-30</span>
                </div>
                <div class='row'>
                    <span class='label'>Mjesto polaganja:</span>
                    <span class='value'>Sarajevo</span>
                </div>
            </section>
            <section class='legal-section'>
                <h2>Zakonska regulativa</h2>
                <p>Prikupljanje, obrada, i čuvanje podataka u ovoj prijavnici vrši se u skladu sa Zakonom o zaštiti ličnih podataka Bosne i Hercegovine.</p>
            </section>
            <div class='signature-box'>
                <p>Potpis</p>
            </div>
        </div>
    </body>
    </html>");

    var htmlContent = htmlBuilder.ToString();

    using (var memoryStream = new MemoryStream())
    {
        using (var pdfWriter = new PdfWriter(memoryStream))
        {
            using (var pdfDocument = new PdfDocument(pdfWriter))
            {
                // Convert HTML to PDF using the memory stream
                HtmlConverter.ConvertToPdf(new MemoryStream(Encoding.UTF8.GetBytes(htmlContent)), pdfDocument);
            }
        }

        return memoryStream.ToArray();
    }
}
 public async Task<byte[]> CreatePdfFromUserDataAsync(string userId)
{
    var user = await _userManager.FindByIdAsync(userId);
    if (user == null)
    {
        throw new Exception("User not found");
    }

    // Prikupi trenutni datum
    string todayDate = DateTime.Now.ToString("yyyy-MM-dd");

    // Prikupi ime korisnika
    string firstName = user.FirstName ?? "Ime";

    // Koristite StringBuilder za kreiranje HTML-a
    var htmlBuilder = new StringBuilder();
    
    htmlBuilder.AppendLine(@"<!DOCTYPE html>
    <html lang='bs'>
    <head>
        <meta charset='UTF-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>PRIMJERAK CIPS PRIJAVNICE</title>
        <style>
            body { font-family: Arial, sans-serif; background-color: #f5f5f5; margin: 0; padding: 20px; }
            .form-container { max-width: 800px; margin: auto; padding: 20px; background-color: #ffffff; border: 1px solid #dddddd; border-radius: 8px; box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1); }
            h1, h2 { text-align: center; color: #333333; }
            h1 { margin-bottom: 20px; }
            h2 { margin: 20px 0; }
            .section-top, .section-middle, .section-main { margin-bottom: 20px; }
            .row { display: flex; justify-content: space-between; margin-bottom: 10px; }
            .label { flex: 1; margin-right: 10px; font-weight: bold; }
            .value { flex: 2; padding: 5px; border: 1px solid #cccccc; border-radius: 4px; background-color: #f5f5f5; }
            p { text-align: center; margin-bottom: 20px; font-style: italic; color: #666666; }
            .legal-section { margin-top: 30px; font-size: 14px; color: #555555; border-top: 1px solid #dddddd; padding-top: 20px; }
            .consent-section { margin-top: 20px; font-size: 14px; }
        </style>
    </head>
    <body>
        <div class='form-container'>
            <h1>PRIMJERAK CIPS PRIJAVNICE</h1>
            <section class='section-top'>
                <div class='row'>
                    <span class='label'>Naziv organa:</span>
                    <span class='value'>eGRAD</span>
                </div>
                <div class='row'>
                    <span class='label'>Broj:</span>
                    <span class='value'>B23</span>
                </div>
                <div class='row'>
                    <span class='label'>Datum:</span>
                    <span class='value'>");

    htmlBuilder.AppendLine(todayDate);
    htmlBuilder.AppendLine(@"</span>
                </div>
            </section>
            <section class='section-middle'>
                <div class='row'>
                    <span class='label'>Ime:</span>
                    <span class='value'>");

    htmlBuilder.AppendLine(firstName);
    htmlBuilder.AppendLine(@"</span>
                </div>
                <div class='row'>
                    <span class='label'>Prezime:</span>
                    <span class='value'>");

    htmlBuilder.AppendLine(user.LastName ?? "Prezime");
    htmlBuilder.AppendLine(@"</span>
                </div>
            </section>
            <section class='section-main'>
                <h2>OBAVJEŠTENJE / OBAVIJEST / ОБАВЈЕШТЕЊЕ</h2>
                <p>da je uveden u evidenciju prebivališta-boravišta sa ličnim/osobnim podacima / да је уведен у евиденцију пребивалишта-боравишта са личним подацима</p>
                <div class='row'>
                    <span class='label'>JMB/МБ:</span>
                    <span class='value'>");

    htmlBuilder.AppendLine(user.JMBG ?? "JMB/МБ");
    htmlBuilder.AppendLine(@"</span>
                </div>
                <div class='row'>
                    <span class='label'>Ime/Име:</span>
                    <span class='value'>");

    htmlBuilder.AppendLine(user.FirstName ?? "Ime/Име");
    htmlBuilder.AppendLine(@"</span>
                </div>
                <div class='row'>
                    <span class='label'>Prezime/Презиме:</span>
                    <span class='value'>");

    htmlBuilder.AppendLine(user.LastName ?? "Prezime/Презиме");
    htmlBuilder.AppendLine(@"</span>
                </div>
                <div class='row'>
                    <span class='label'>Spol/Пол:</span>
                    <span class='value'>");

    htmlBuilder.AppendLine(user.Pol ?? "Spol/Пол");
    htmlBuilder.AppendLine(@"</span>
                </div>
                <div class='row'>
                    <span class='label'>Datum rođenja:</span>
                    <span class='value'>");

    htmlBuilder.AppendLine(user.DatumRodjenja.ToString("yyyy-MM-dd") ?? "Datum rođenja");
    htmlBuilder.AppendLine(@"</span>
                </div>
                <div class='row'>
                    <span class='label'>Općina prebivališta/Општина пребивалишта:</span>
                    <span class='value'>");

    htmlBuilder.AppendLine(user.OpstinaPrebivalista ?? "Općina prebivališta/Општина пребивалишта");
    htmlBuilder.AppendLine(@"</span>
                </div>
                <div class='row'>
                    <span class='label'>Adresa prebivališta/Адреса пребивалишта:</span>
                    <span class='value'>");

    htmlBuilder.AppendLine(user.AdresaPrebivalista ?? "Adresa prebivališta/Адреса пребивалишта");
    htmlBuilder.AppendLine(@"</span>
                </div>
            </section>
            <section class='legal-section'>
                <h2>Zakonska regulativa</h2>
                <p>Prikupljanje, obrada, i čuvanje podataka u ovoj prijavnici vrši se u skladu sa Zakonom o zaštiti ličnih podataka Bosne i Hercegovine ('Službeni glasnik BiH', br. 49/06, 76/11) i Zakonom o prebivalištu i boravištu državljana Bosne i Hercegovine ('Službeni glasnik BiH', br. 32/01, 56/08, 58/15).</p>
                <p>Podaci iz ove prijavnice koriste se isključivo za potrebe evidentiranja prebivališta-boravišta i neće biti korišteni u druge svrhe bez vaše saglasnosti, osim u slučajevima predviđenim zakonom.</p>
            </section>
            <section class='consent-section'>
                <h2>Saglasnost</h2>
                <p>Molimo vas da potvrdite vašu saglasnost za obradu podataka u skladu sa navedenim zakonskim propisima:</p>
                <div class='row'>
                    <input type='checkbox' name='consent' id='consent'>
                    <label for='consent'>Saglasan/saglasna sam sa obradom mojih ličnih podataka.</label>
                </div>
            </section>
        </div>
    </body>
    </html>");

    var htmlContent = htmlBuilder.ToString();

    using (var memoryStream = new MemoryStream())
    {
        using (var pdfWriter = new PdfWriter(memoryStream))
        {
            using (var pdfDocument = new PdfDocument(pdfWriter))
            {
                // Convert HTML to PDF using the memory stream
                HtmlConverter.ConvertToPdf(new MemoryStream(Encoding.UTF8.GetBytes(htmlContent)), pdfDocument);
            }
        }

        return memoryStream.ToArray();
    }

  


}





}

    

