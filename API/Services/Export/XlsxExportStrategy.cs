
namespace API.Services.Export
{
    public class XlsxExportStrategy<T> : IExportStrategy<T>
    {
        public byte[] ExportData(List<T> data)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Data");
                var properties = typeof(T).GetProperties();

                // Add headers (property names)
                for (int col = 0; col < properties.Length; col++)
                {
                    worksheet.Cells[1, col + 1].Value = properties[col].Name;
                }

                // Add values
                int row = 2;
                foreach (var item in data)
                {
                    for (int col = 0; col < properties.Length; col++)
                    {
                        worksheet.Cells[row, col + 1].Value = properties[col].GetValue(item)?.ToString();
                    }
                    row++;
                }

                return package.GetAsByteArray();
            }
        }

    }
}