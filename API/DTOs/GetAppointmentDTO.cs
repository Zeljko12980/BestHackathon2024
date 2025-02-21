using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class GetAppointmentDTO
    {
        public int AppointmentId { get; set; }
      
        public DateTime AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public string ServiceType { get; set; }
        public string ServiceSubType { get; set; }
        public string UserId { get; set; }
    }
}