
namespace API.Models
{
    public class RequestSubtype
    {
    
        public int RequestSubtypeID { get; set; }
        public string RequestSubtypeName { get; set; }
        public int RequestTypeID { get; set; }  // Dodajte ovo polje kao strano ključno
        public RequestType RequestType { get; set; }
    }
}