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
            public  string Pol { get; set; }
            public  DateOnly DatumRodjenja {get;set;}

            public  string AdresaPrebivalista {get;set;}
            public  string OpstinaPrebivalista{get;set;}
            public ICollection<Appointment> Appointments { get; set; }
            public  ICollection<Request>? Requests{get;set;}
         
         

        }

}