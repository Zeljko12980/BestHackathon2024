namespace API.Services.SchoolClass
{
  public interface ISchoolClassService
  {
    Task<List<ClassSummaryDto>> GetClassSummariesAsync();
  }
}