
namespace API.Services.PDF
{
public class PDFService : IPDFService
{
    private readonly UserManager<User> _userManager;

    public PDFService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public Task<byte[]> CreateDriverLicensePdfFromUserDataAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<byte[]> CreatePassportPdfFromUserDataAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<byte[]> CreatePdfFromUserDataAsync(string userId)
    {
        throw new NotImplementedException();
    }
}


}



    

