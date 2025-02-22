


namespace API.Services.Api
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _flaskApiUrl1;
        private readonly string _flaskApiUrl2;
        private readonly string _flaskApiUrl3;
        private const string FlaskServerUrl = "http://localhost:5000/prepoznaj-lice";
        private readonly UserManager<User> _userManager;

        public ApiService(HttpClient httpClient, IConfiguration configuration,UserManager<User> userManager)
        {
            _httpClient = httpClient;
            _flaskApiUrl1 = configuration["FlaskApi:Url"];  // Učitavanje URL-a iz konfiguracije
            _flaskApiUrl2 = configuration["FlaskApi:Url2"];
            _userManager= userManager;
        }

    public async Task<User?> PrepoznajLiceAsync()
{
     try
        {
            var response = await _httpClient.PostAsync(FlaskServerUrl, null);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var result = await response.Content.ReadAsStringAsync();
            var cleanedResult = result.Trim('"');
            cleanedResult = Regex.Replace(cleanedResult, "[^a-zA-Z]", "");

            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.UserName.ToLower() == cleanedResult.ToLower());

            return user;
        }
        catch (Exception)
        {
            return null;
        }
}

        public async Task<string> GameOnePlayAsync(string userId)
        {
            try
        {
            var content = new StringContent("{}", Encoding.UTF8, "application/json");
         
            
            var response = await _httpClient.GetAsync(_flaskApiUrl2+userId);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();

                // Deserializacija odgovora u anonimni objekat
                var responseObject = JsonConvert.DeserializeObject<dynamic>(responseString);

                // Dobijanje imena i uklanjanje nepotrebnih karaktera
                string ime = responseObject.ToString().Trim('"', '\n');

                return ime;
            }
            else
            {
                return "Greška: " + response.StatusCode;
            }
        }
        catch (Exception ex)
        {
            return "Izuzetak: " + ex.Message;
        }
        }

    }
}
