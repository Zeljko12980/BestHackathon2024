namespace API.Services.Student
{
    public interface IStudentService
    {
        Task<List<User>> GetStudentsByClassAsync(int classId);
    }
}