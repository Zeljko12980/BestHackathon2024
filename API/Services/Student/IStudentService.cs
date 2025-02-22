namespace API.Services.Student
{
    public interface IStudentService
    {
        Task<ActionResult<IEnumerable<User>>> GetAllStudents();
        Task<List<User>> GetStudentsByClassAsync(int classId);
    }
}