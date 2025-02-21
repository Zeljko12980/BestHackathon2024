namespace API.signalRModels
{
    public class SignalRequest
    {
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public object Data { get; set; }

        public SignalRequest(string sender, object data)
        {
            Sender = sender;
            Data = data;
        }
    }
}