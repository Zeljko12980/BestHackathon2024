using System;
using System.Collections.Generic;

namespace API.DTOs
{
    public class UserDTO
    {
        public required string Email { get; set; }
        public required string Token { get; set; }
        public required string UserName { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string JMBG { get; set; }
        public required string Id { get; set; }
        public List<string>? Roles { get; set; } = null;

        // Novo - povezano sa školskim razredom
        public int? SchoolClassId { get; set; } // ID razreda
        public string? SchoolClassName { get; set; } // Naziv razreda
        public List<string>? TeachingClasses { get; set; } // Razredi koje profesor predaje
    }
}
