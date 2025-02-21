using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Uveri se da imaš referencu na Entity Framework
using API.Data; // Uveri se da imaš referencu na svoj kontekst
using API.Models; // Uveri se da imaš referencu na svoj Appointment model
using OfficeOpenXml; // Uveri se da imaš referencu na EPPlus

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class XLSXExport : ControllerBase
    {
        private readonly StoreContext _context;

        public XLSXExport(StoreContext context)
        {
            _context = context;
        }

        private async Task<List<Appointment>> GetDataForExport()
        {
            return await _context.Appointments.ToListAsync();
        }

        [HttpGet("export-xlsx")]
        public async Task<IActionResult> ExportXlsx()
        {
            var data = await GetDataForExport();
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Appointments");

            // Dodaj zaglavlja
            worksheet.Cells[1, 1].Value = "AppointmentId";
            worksheet.Cells[1, 2].Value = "UserEmail";
            worksheet.Cells[1, 3].Value = "CreatedAt";
            worksheet.Cells[1, 4].Value = "AppointmentDate";
            worksheet.Cells[1, 5].Value = "AppointmentTime";
            worksheet.Cells[1, 6].Value = "ServiceType";
            worksheet.Cells[1, 7].Value = "ServiceSubType";

            // Dodaj podatke
            int row = 2;
            foreach (var appointment in data)
            {
                worksheet.Cells[row, 1].Value = appointment.AppointmentId;
                worksheet.Cells[row, 2].Value = appointment.UserEmail;
                worksheet.Cells[row, 3].Value = appointment.CreatedAt;
                worksheet.Cells[row, 4].Value = appointment.AppointmentDate;
                worksheet.Cells[row, 5].Value = appointment.AppointmentTime;
                worksheet.Cells[row, 6].Value = appointment.ServiceType;
                worksheet.Cells[row, 7].Value = appointment.ServiceSubType;
                row++;
            }

            // Auto-fit kolone
            worksheet.Cells.AutoFitColumns();

            // Vraćanje XLSX fajla
            var fileContents = package.GetAsByteArray();
            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "appointments.xlsx");
        }
    }
}
