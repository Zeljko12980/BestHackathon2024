
namespace API.Controllers
{
    public class ExportController:BaseAPIController
    {
        private readonly ExportService _exportService;

        public ExportController(ExportService exportService)
        {
            _exportService = exportService;
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportData<T>(string format)
        {
            // Assuming you have a method to fetch data for the export
            List<T> data = await GetDataForExport<T>();

            var fileContent = await _exportService.ExportDataAsync(data, format);

            string contentType;
            string fileName;

            switch (format.ToLower())
            {
                case "csv":
                    contentType = "text/csv";
                    fileName = "data.csv";
                    break;
                case "pdf":
                    contentType = "application/pdf";
                    fileName = "data.pdf";
                    break;
                case "xlsx":
                    contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    fileName = "data.xlsx";
                    break;
                default:
                    return BadRequest("Invalid export format");
            }

            return File(fileContent, contentType, fileName);
        }

        // Example of a method to retrieve data (replace this with your actual data fetching logic)
        private async Task<List<T>> GetDataForExport<T>()
        {
            // You should replace this with your actual data fetching logic
            return new List<T>();
        }
    }
}