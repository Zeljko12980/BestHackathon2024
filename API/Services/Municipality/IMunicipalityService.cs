using  API.Models;
using API.Data;
namespace API.Services.Interfaces
{
    public interface IMunicipalityService
    {
        public Task AddMunicipality(Municipality municipality);
        public Task<Municipality> GetMunicipality(string zipCode);
        public Task<List<Municipality>> GetAllMunicipalities();
        public Task UpdateMunicipality(string zipCode, Municipality updatedMunicipality);
        public Task DeleteMunicipality(string zipCode);

    }
}