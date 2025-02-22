
namespace API.Services.Export
{
    public class PdfExportStrategy<T> : IExportStrategy<T>
    {
        public byte[] ExportData(List<T> data)
        {
            var memoryStream = new MemoryStream();
            var document = new iTextSharp.text.Document();
            var writer = PdfWriter.GetInstance(document, memoryStream);

            document.Open();
            var table = new PdfPTable(typeof(T).GetProperties().Length);

            // Add headers (property names)
            foreach (var property in typeof(T).GetProperties())
            {
                table.AddCell(property.Name);
            }

            // Add values
            foreach (var item in data)
            {
                foreach (var property in typeof(T).GetProperties())
                {
                    table.AddCell(property.GetValue(item)?.ToString());
                }
            }

            document.Add(table);
            document.Close();

            return memoryStream.ToArray();
        }

    }
}