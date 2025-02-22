namespace API.Services.Interfaces
{
    public interface IExportStrategy<T>
    {
        byte[] ExportData(List<T> data);
    }
}