
namespace API.Services.Api
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _flaskApiUrl;

        public ApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _flaskApiUrl = configuration["FlaskApi:Url"];  // Učitavanje URL-a iz konfiguracije
        }

       public async Task<string> PrepoznajLiceAsync()
    {
        try
        {
            var content = new StringContent("{}", Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_flaskApiUrl, content);

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
