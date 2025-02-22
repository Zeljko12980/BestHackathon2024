namespace API.Services.Export
{
    public class ExportService
    {
         public async Task<byte[]> ExportDataAsync<T>(List<T> data, string format)
        {
            IExportStrategy<T> exportStrategy;

            switch (format.ToLower())
            {
                case "csv":
                    exportStrategy = new CsvExportStrategy<T>();
                    break;
                case "pdf":
                    exportStrategy = new PdfExportStrategy<T>();
                    break;
                case "xlsx":
                    exportStrategy = new XlsxExportStrategy<T>();
                    break;
                default:
                    throw new InvalidOperationException("Unsupported export format");
            }

            return exportStrategy.ExportData(data);
        }
    }
}