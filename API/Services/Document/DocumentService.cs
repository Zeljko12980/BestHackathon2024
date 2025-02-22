
using Tesseract;

namespace API.Services.Document
{
    public class DocumentService : IDocumentService
    {

        public DocumentService()
        {
            
        }
       public async Task<(string JMB, string RecognizedText)> ProcessImageAsync(IFormFile image)
    {
        if (image == null || image.Length == 0)
            throw new ArgumentException("No image uploaded.");

        string filePath = await SaveTempFile(image);
        string recognizedText = RecognizeText(filePath);
        string jmb = ExtractJMB(recognizedText);

        return (jmb, recognizedText);
    }

    private async Task<string> SaveTempFile(IFormFile image)
    {
        string filePath = Path.GetTempFileName();
        using var stream = new FileStream(filePath, FileMode.Create);
        await image.CopyToAsync(stream);
        return filePath;
    }

    private string RecognizeText(string imagePath)
    {
        using var engine = new TesseractEngine(@"./tessdata", "bos+srp+srp_latn", EngineMode.Default);
        using var img = Pix.LoadFromFile(imagePath);
        using var page = engine.Process(img);
        return page.GetText();
    }

    private string ExtractJMB(string text)
    {
        text = Regex.Replace(text, @"\s+", ""); // Ukloni praznine i specijalne znakove
        var match = Regex.Match(text, @"(?<!\d)\d{13}(?!\d)");
        return match.Success ? match.Value : null;
    }

    }
}