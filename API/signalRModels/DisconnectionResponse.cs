namespace API.signalRModels
{
    public class DisconnectionResponse
    {
        public string Sender { get; set; }
        public DisconnectionResponse(string sender)
        {
            Sender = sender;
        }
    }
}