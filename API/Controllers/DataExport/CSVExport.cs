using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Uveri se da imaš referencu na Entity Framework
using API.Data; // Uveri se da imaš referencu na svoj kontekst
using API.Models; // Uveri se da imaš referencu na svoj Appointment model

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CSVExport : ControllerBase
    {
        private readonly StoreContext _context;

        public CSVExport(StoreContext context)
        {
            _context = context;
        }

        // Ovaj metod dobija postojeće appointments iz baze
        private async Task<List<Appointment>> GetDataForExport()
        {
            return await _context.Appointments.ToListAsync(); // Preuzmi sve appointment-e iz baze
        }

        // Ovaj metod generiše CSV string iz podataka
        private string GenerateCsv(List<Appointment> data)
        {
            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("AppointmentId,UserEmail,CreatedAt,AppointmentDate,AppointmentTime,ServiceType,ServiceSubType,UserId"); // Zaglavlja CSV-a

            foreach (var appointment in data)
            {
                csvBuilder.AppendLine($"{appointment.AppointmentId},{appointment.UserEmail},{appointment.CreatedAt},{appointment.AppointmentDate},{appointment.AppointmentTime},{appointment.ServiceType},{appointment.ServiceSubType},{appointment.UserId}"); // Vrednosti
            }

            return csvBuilder.ToString();
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportCsv()
        {
            var data = await GetDataForExport(); // Nabavi podatke koje želiš da eksportuješ

            var csv = GenerateCsv(data); // Generiši CSV string iz podataka

            var bytes = Encoding.UTF8.GetBytes(csv);
            var stream = new MemoryStream(bytes);

            return File(stream, "text/csv", "appointments.csv"); // Vraća CSV fajl sa odgovarajućim header-ima
        }
    }
}
