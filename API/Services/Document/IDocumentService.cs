namespace API.Services.Document
{
    public interface IDocumentService
    {
        Task<(string JMB, string RecognizedText)> ProcessImageAsync(IFormFile image);
    }
}