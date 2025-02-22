
namespace API.Services.Export
{
    public class CsvExportStrategy<T>:IExportStrategy<T>
    {
        public byte[] ExportData(List<T> data)
        {
            var csvBuilder = new StringBuilder();
            var properties = typeof(T).GetProperties();

            // Add headers (property names)
            foreach (var property in properties)
            {
                csvBuilder.Append(property.Name).Append(",");
            }
            csvBuilder.AppendLine();

            // Add values
            foreach (var item in data)
            {
                foreach (var property in properties)
                {
                    csvBuilder.Append(property.GetValue(item)?.ToString()).Append(",");
                }
                csvBuilder.AppendLine();
            }

            return Encoding.UTF8.GetBytes(csvBuilder.ToString());
        }
    }
}