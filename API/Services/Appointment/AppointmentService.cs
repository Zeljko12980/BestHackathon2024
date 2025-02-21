using API.Services;
using API.Services.Interfaces;
using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace API.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly StoreContext _context;

        public AppointmentService(StoreContext context)
        {
            _context=context;
        }

        public async Task AddAppointment(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            _context.SaveChanges();
        }
        public async Task<Appointment> GetAppointment(int appointmentId)
        {
            var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.AppointmentId  == appointmentId);
            return appointment;
        }
        public async Task UpdateAppointment(int appointmentId, Appointment updatedAppointment)
        {
            var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.AppointmentId  == appointmentId);

            appointment.AppointmentId = updatedAppointment.AppointmentId;
            appointment.UserEmail = updatedAppointment.UserEmail;
            appointment.AppointmentTime = updatedAppointment.AppointmentTime;
            appointment.AppointmentDate = updatedAppointment.AppointmentDate;
            appointment.ServiceType = updatedAppointment.ServiceType;
            appointment.ServiceSubType = updatedAppointment.ServiceSubType;

            _context.SaveChanges();
        }
        public async Task DeleteAppointment(int appointmentId)
        {
            var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.AppointmentId  == appointmentId);
            if (appointment == null)
            {
                throw new Exception("Appointment with this id does not exist!!");
            }

            _context.Appointments.Remove(appointment);
            _context.SaveChanges();
        }

        
        
    }
}