public class StudentsController : BaseAPIController
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet("class/{classId}")]
    public async Task<ActionResult<IEnumerable<User>>> GetStudentsByClass(int classId)
    {
        var students = await _studentService.GetStudentsByClassAsync(classId);

        if (students == null || students.Count == 0)
        {
            return NotFound("Nema učenika u ovom razredu.");
        }

        return Ok(students);
    }

     [HttpGet("stranica")]
    public async Task<ActionResult> GetAllUsers(int pageNumber = 1, int pageSize = 5, string? searchTerm = null, string? userId = null)
    {
        var result = await _studentService.GetAllUsersAsync(pageNumber, pageSize, searchTerm, userId);
        
        return Ok(new
        {
            Users = result.Users,
            TotalCount = result.TotalCount,
            TotalPages = result.TotalPages
        });
    }
}