using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services.Interfaces
{
    public interface IAppointmentService
    {
        public Task AddAppointment(Appointment appointment);
        public Task<Appointment> GetAppointment(int appointmentId);
        public Task UpdateAppointment(int appointmentId, Appointment updatedAppointment);
        public Task DeleteAppointment(int appointmentId);
    }
}