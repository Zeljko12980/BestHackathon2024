using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
   public class UserDTO
    {
    public required string Email { get; set; }
    public required string Token { get; set; }
    public required string UserName { get; set; }

    public required string FirstName { get; set; }
    public required string LastName { get; set; }

    public  string AdresaPrebivalista {get;set;}
    public  string OpstinaPrebivalista{get;set;}
    public  DateTime? DatumRodjenja{get;set;} = null;
  
    public required string JMBG { get; set; }
    public List<string>? Roles { get; set; } = null;
    public required string Id {get; set; }

    }
}
