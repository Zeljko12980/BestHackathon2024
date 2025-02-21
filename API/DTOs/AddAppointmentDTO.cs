using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class AddAppointmentDTO
    {
        public string ServiceType { get; set; }
        public string ServiceSubType { get; set; }
        public string UserEmail { get; set; }
         public DateTime AppointmentDate { get; set; } // Datum
        public string AppointmentTime { get; set; }
    }
}