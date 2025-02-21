using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class UpdateUserDTO
    {
            public required string FirstName { get; set; }
            public required string LastName { get; set; }
            public required string Email { get; set; }
            public required string UserName { get; set; }
            public  required string AdresaPrebivalista {get;set;}
            public  required string OpstinaPrebivalista{get;set;}
    }
}