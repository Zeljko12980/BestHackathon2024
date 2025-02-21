using API.Services.Interfaces;
using API.Models;
using API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;  // Required for EF Core
using System.Collections.Generic;  // Required for List<>
using System.Linq;  // Required for LINQ methods like ToList()
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace API.Services
{
    public class MunicipalityService : IMunicipalityService
    {
        private readonly StoreContext _context;

        public MunicipalityService(StoreContext context)
        {
            _context=context;
        }

        public async Task AddMunicipality(Municipality municipality)
        {
            _context.Municipalities.Add(municipality);
            _context.SaveChanges();
        }

        public async Task<List<Municipality>> GetAllMunicipalities()
        {
            var municipalities = await _context.Municipalities.ToListAsync();

            return municipalities;
        }
        public async Task<Municipality> GetMunicipality(string zipCode)
        {
            var municipality = await _context.Municipalities.FirstOrDefaultAsync(m => m.ZipCode == zipCode);
            return municipality;
        }
        public async Task UpdateMunicipality(string zipCode, Municipality updatedMunicipality)
        {
            var municipality = await _context.Municipalities.FirstOrDefaultAsync(m => m.ZipCode == zipCode);

            if(municipality == null)
            {
                Console.WriteLine("Error");
            }

            municipality.MunicipalityName = updatedMunicipality.MunicipalityName;
            _context.SaveChanges();
        }
        public async Task DeleteMunicipality(string zipCode)
        {
            var municipality = await _context.Municipalities.FirstOrDefaultAsync(m => m.ZipCode == zipCode);
            if (municipality == null)
            {
                throw new Exception("Municipality with this zipCode does not exist!!");
            }

            _context.Municipalities.Remove(municipality);
            _context.SaveChanges();
        }

        
    }
}