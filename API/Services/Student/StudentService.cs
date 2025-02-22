namespace API.Services.Student
{
    public class StudentService:IStudentService
    {
        private readonly UserManager<User> _userManager;

        public StudentService(UserManager<User> userManager)
        {
          _userManager = userManager;
        }

        public async Task<List<User>> GetStudentsByClassAsync(int classId)
        {
          return await _userManager.Users
              .Where(u => u.SchoolClassId == classId)
              .ToListAsync();
        }
    }
}