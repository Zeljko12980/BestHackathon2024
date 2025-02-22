using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class SchoolClass
{
    public int Id { get; set; }
    public string Name { get; set; }  // Naziv razreda (npr. 1A, 2B)
    public List<User> Students { get; set; }  // Lista učenika koji pohađaju ovaj razred
    public List<User> Teachers { get; set; }  // Lista profesora koji predaju u ovom razredu
}

}