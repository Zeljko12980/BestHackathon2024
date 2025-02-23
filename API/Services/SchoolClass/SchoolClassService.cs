namespace API.Services.SchoolClass
{
    public class SchoolClassService:ISchoolClassService
    {
        private readonly StoreContext _context;

    public SchoolClassService(StoreContext context)
    {
        _context = context;
    }

    public async Task<List<ClassSummaryDto>> GetClassSummariesAsync()
    {
        return await _context.SchoolClasses
            .Select(c => new ClassSummaryDto
            {
                ClassName = c.Name,
                TotalScore = c.Students.Sum(s => s.Score),
                StudentCount = c.Students.Count()
            })
            .ToListAsync();
    }
    }
}