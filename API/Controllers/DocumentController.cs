public class DocumentController :BaseAPIController
{
     private readonly IDocumentService _documentService;

    public DocumentController(IDocumentService documentService)
    {
        _documentService = documentService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile image)
    {
        try
        {
            var result = await _documentService.ProcessImageAsync(image);
            return Ok(new { JMB = result.JMB, RecognizedText = result.RecognizedText });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Došlo je do greške pri obradi slike.");
        }
    }

}
