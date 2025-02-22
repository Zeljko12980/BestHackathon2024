
namespace API.Services.Twilio
{
    public class TwilioService : ITwilioService
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _fromPhoneNumber;

        public TwilioService(IConfiguration configuration)
        {
            _accountSid = configuration["Twilio:AccountSid"];
            _authToken = configuration["Twilio:AuthToken"];
            _fromPhoneNumber = configuration["Twilio:FromPhoneNumber"];
            TwilioClient.Init(_accountSid, _authToken);
        }
        
        public void SendSms(string to, string message)
        {
            var messageOptions = new CreateMessageOptions(new PhoneNumber(to))
            {
            From = new PhoneNumber(_fromPhoneNumber),
            Body = message
            };
            
            MessageResource.Create(messageOptions);
        }

    }
}