namespace API.Controllers
{
    public class SchoolClassController:BaseAPIController
    {
        private readonly ISchoolClassService _schoolClassService;

        
         public SchoolClassController(ISchoolClassService schoolClassService)
         {
            _schoolClassService = schoolClassService;
         }

         [HttpGet("summaries")]
         public async Task<ActionResult<List<ClassSummaryDto>>> GetClassSummaries()
         {
             var classSummaries = await _schoolClassService.GetClassSummariesAsync();
             return Ok(classSummaries);
         }
    }
}