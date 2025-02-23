
namespace API.Services.Student
{
    public class StudentService : IStudentService
    {
        private readonly UserManager<User> _userManager;

        public StudentService(UserManager<User> userManager)
        {
          _userManager = userManager;
        }

        public async Task<ActionResult<IEnumerable<User>>> GetAllStudents()
        {
            return await _userManager.Users
            .Where(u => u.SchoolClassId > 0) // Filtriranje korisnika prema SchoolClassId // Uključivanje SchoolClass entiteta
            .ToListAsync();


        }

        public async Task<List<User>> GetStudentsByClassAsync(int classId)
        {
          return await _userManager.Users
              .Where(u => u.SchoolClassId == classId)
              .ToListAsync();
        }


        public async Task<PaginatedResult<User>> GetAllUsersAsync(int pageNumber = 1, int pageSize = 5, string? searchTerm = null, string? userId = null)
        {
            var query = _userManager.Users.AsQueryable();


            // Ako postoji searchTerm, filtriramo korisnike po imenu ili emailu
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(u => u.UserName.Contains(searchTerm) || u.Email.Contains(searchTerm));
            }

            // Ako postoji userId, možemo dodatno filtrirati (ako je potrebno)
            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(u => u.Id == userId);
            }

            // Ukupan broj rezultata pre paginacije
            var totalCount = await query.CountAsync();

            // Paginacija rezultata
            var users = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Vraćamo paginirane rezultate
            return new PaginatedResult<User>
            {
                Users = users,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }

    }
}