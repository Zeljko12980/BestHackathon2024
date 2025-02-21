

namespace API.Models



{
    public class Request
    {
     
        public int RequestId { get; set;}
        public DateTime CreatedAt { get; set;} = DateTime.Now;
        public bool Approved { get; set; } = false;
        public string UserId { get; set; }
        public User User { get; set; }
        public int RequestTypeID { get; set; }  // Dodajte ovo polje kao strano ključno
        public RequestType RequestType { get; set; }

        public ICollection<RequestType> RequestTypeList{ get; set;}
    }
}