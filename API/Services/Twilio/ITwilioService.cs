namespace API.Services.Twilio
{
    public interface ITwilioService
    {
        void SendSms(string to, string message);
    }
}