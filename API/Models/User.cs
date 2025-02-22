using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace API.Models
{
  
      public class User : IdentityUser
        {
            public required string FirstName { get; set; }
            public required string LastName { get; set; }
            public required string JMBG { get; set; }

            // Veza sa razredom koji učenik pohađa
            public int? SchoolClassId { get; set; }
            public SchoolClass SchoolClass { get; set; }  // Razred koji učenik pohađa

            // Lista razreda koje profesor predaje
            public List<SchoolClass> TeachingClasses { get; set; } = new List<SchoolClass>(); // Razredi koje profesor predaje (ako je korisnik profesor)
        }

}