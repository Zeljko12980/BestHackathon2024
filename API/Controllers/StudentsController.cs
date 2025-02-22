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
}