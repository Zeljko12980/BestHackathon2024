using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Appointment
    {
         public int AppointmentId { get; set; }
        
         public string UserEmail { get; set; }
        public DateTime CreatedAt { get; set; } =  DateTime.Now;
         public DateTime AppointmentDate { get; set; } // Datum
        public string AppointmentTime { get; set; }
        public string ServiceType { get; set; }
        public string ServiceSubType { get; set; }
         public string UserId { get; set; }
    }
}