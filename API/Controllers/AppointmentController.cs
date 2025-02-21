using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly StoreContext _context;
      
        private readonly UserManager<User> _userManager;

        public AppointmentController(StoreContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
          
        }

        
        [HttpGet("GetAppointments")]
        public async Task<ActionResult<List<Appointment>>> GetAppointments()
        {
             
            return await _context.Appointments.ToListAsync();
        }

        [HttpGet("GetUserAppointments")]
        public async Task<ActionResult<List<Appointment>>> GetUserAppointments()
        {
            var user = await _userManager.FindByNameAsync(User.Identity?.Name!);
            if (user == null) return Unauthorized();

            // Filtriranje termina na osnovu ID-a korisnika
            var appointments = await _context.Appointments
                .Where(a => a.UserId == user.Id)
                .ToListAsync();

            return Ok(appointments);
        }

      
        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> GetAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);

        if (appointment == null)
        {
            return NotFound();
        }

        return appointment;

        
        }
        [HttpGet("appointment{name}")]
        public async Task<ActionResult<List<Appointment>>> GetAppointmentByName(string name)
        {
            var appointments = await _context.Appointments.Where(a => a.ServiceType == name).ToListAsync();


            return appointments;
        }

        
        [HttpPost("AddAppointments")]
        public async Task<ActionResult<List<Appointment>>> PostAppointment(AddAppointmentDTO appointmentDto)
        {
            var user = await _userManager.FindByNameAsync(User.Identity?.Name!);
            if (user == null) return Unauthorized();


              var existingAppointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.ServiceType == appointmentDto.ServiceType 
                && a.AppointmentDate == appointmentDto.AppointmentDate && a.AppointmentTime == appointmentDto.AppointmentTime);

            if (existingAppointment != null)
            {
                return BadRequest("Termin za ovu uslugu u ovom vremenu je već zauzet.");
            }
            
            var appointment = new Appointment
            {
                AppointmentDate = appointmentDto.AppointmentDate,
                AppointmentTime=appointmentDto.AppointmentTime,
                CreatedAt = DateTime.UtcNow,
                ServiceType = appointmentDto.ServiceType,
                ServiceSubType = appointmentDto.ServiceSubType,
                UserId = user.Id ,
                UserEmail = appointmentDto.UserEmail,
                
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            var appointments = await _context.Appointments.ToListAsync();
            return appointments;
        }

        
        
        [HttpDelete("delete{id}")]
        public async Task<ActionResult> DeleteAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return Ok();
        }

       
        [HttpDelete("DeleteUserAppointment/{id}")]
        public async Task<ActionResult> DeleteUserAppointment(int id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity?.Name!);
            if (user == null) return Unauthorized();

          
            var appointment = await _context.Appointments
                .Where(a => a.AppointmentId == id && a.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (appointment == null)
            {
                return NotFound("Appointment not found or you do not have permission to delete it.");
            }

       
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return Ok("Appointment deleted successfully.");
        }
    }
}