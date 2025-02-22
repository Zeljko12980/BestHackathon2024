namespace API.Services.Student
{
    public class StudentService:IStudentService
    {
       private readonly StoreContext _context;
    private readonly UserManager<User> _userManager;

    public StudentService(StoreContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<List<User>> GetStudentsByClassAsync(int classId)
        {
          return await _userManager.Users
              .Where(u => u.SchoolClassId == classId)
              .ToListAsync();
        }
    public async Task<PaginatedResult<User>> GetAllUsersAsync(int pageNumber, int pageSize, string? searchTerm, string? userId)
    {
        var query = _context.Users.AsQueryable();

        // Apply search filter if searchTerm is provided
        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(u => u.FirstName.Contains(searchTerm) || 
                                      u.LastName.Contains(searchTerm) || 
                                      u.JMBG.Contains(searchTerm));
        }

        // If a userId is provided, filter based on the user's SchoolClass or TeachingClasses
        if (!string.IsNullOrEmpty(userId))
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                // If the user is a student (has a SchoolClassId), filter by that SchoolClass
                if (user.SchoolClassId.HasValue)
                {
                    query = query.Where(u => u.SchoolClassId == user.SchoolClassId);
                }
                // If the user is a teacher (has TeachingClasses), filter by those classes
                if (user.TeachingClasses.Any())
                {
                    var teachingClassIds = user.TeachingClasses.Select(tc => tc.Id).ToList();
                    query = query.Where(u => u.TeachingClasses.Any(tc => teachingClassIds.Contains(tc.Id)));
                }
            }
        }

        // Get the filtered users
        var users = await query
            .Skip((pageNumber - 1) * pageSize) // Skip the users for previous pages
            .Take(pageSize) // Take the users for the current page
            .ToListAsync();

        // Calculate the total number of users after filtering
        var usersLength = await query.CountAsync();

        // Calculate the total number of pages
        decimal totalPages = Math.Ceiling((decimal)usersLength / pageSize);

        // Return the result
        return new PaginatedResult<User>
        {
            Users = users,
            TotalCount = usersLength,
            TotalPages = totalPages
        };
    }
    }
}