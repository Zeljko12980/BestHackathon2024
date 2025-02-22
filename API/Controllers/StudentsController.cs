using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly StoreContext _context;

        public StudentsController(UserManager<User> userManager, StoreContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: api/students/class/{classId}
   [HttpGet("class/{classId}")]
public async Task<ActionResult<IEnumerable<User>>> GetStudentsByClass(int classId)
{
    // Uzimamo sve korisnike koji pripadaju datom razredu
    var students = await _userManager.Users
        .Where(u => u.SchoolClassId == classId)
        .ToListAsync();

    if (students == null || !students.Any())
    {
        return NotFound("Nema učenika u ovom razredu.");
    }

    return Ok(students);
}




    }
}
