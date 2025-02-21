using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Uveri se da imaš referencu na Entity Framework
using API.Data; // Uveri se da imaš referencu na svoj kontekst
using API.Models; // Uveri se da imaš referencu na svoj Appointment model
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties; // Dodaj ovo za dodatne opcije

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PDFExport : ControllerBase
    {
        private readonly StoreContext _context;

        public PDFExport(StoreContext context)
        {
            _context = context;
        }

        private async Task<List<Appointment>> GetDataForExport()
        {
            return await _context.Appointments.ToListAsync();
        }

        [HttpGet("export-pdf")]
        public async Task<IActionResult> ExportPdf()
        {
            var data = await GetDataForExport();
            using var stream = new MemoryStream();
            var writer = new PdfWriter(stream);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            // Postavljanje margina
            document.SetMargins(20, 20, 20, 20); // gornja, desna, donja, leva

            // Dodaj naslov
            document.Add(new Paragraph("Lista Termina")
                .SetFontSize(20)
                .SetBold()
                .SetTextAlignment(TextAlignment.CENTER)); // Centriraj naslov

            // Dodaj tabelu
            var table = new Table(new float[] { 1, 2, 2, 2, 2, 3 }); // Kolone sa težinama
            table.SetFixedLayout(); // Podesi da tabela koristi fiksni raspored

            // Dodaj zaglavlja
            table.AddHeaderCell("AppointmentId");
            table.AddHeaderCell("UserEmail");
           
            table.AddHeaderCell("AppointmentDate");
            table.AddHeaderCell("AppointmentTime");
            table.AddHeaderCell("ServiceType");
            table.AddHeaderCell("ServiceSubType");

            // Dodaj podatke
            foreach (var appointment in data)
            {
                table.AddCell(appointment.AppointmentId.ToString());
                table.AddCell(appointment.UserEmail);
                
                table.AddCell(appointment.AppointmentDate.ToString());
                table.AddCell(appointment.AppointmentTime);
                table.AddCell(appointment.ServiceType);
                table.AddCell(appointment.ServiceSubType);
            }

            document.Add(table);
            document.Close();

            // Vraćanje PDF fajla
            var fileContents = stream.ToArray();
            return File(fileContents, "application/pdf", "appointments.pdf");
        }
    }
}
