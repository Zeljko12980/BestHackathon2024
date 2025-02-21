
namespace API.Models
{
    public class RequestType
    {
    
        public int RequestTypeID { get; set; }
        public string RequestTypeName { get; set; }
        public ICollection<RequestSubtype> RequestSubTypeList{ get; set;}
    }
}