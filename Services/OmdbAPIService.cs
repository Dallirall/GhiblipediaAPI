using GhiblipediaAPI.Models;
using Newtonsoft.Json;

namespace GhiblipediaAPI.Services
{
    public class OmdbAPIService
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<OmdbMovie> GetOmdbMovie(string movieTitle)
        {

            string apiKey = "52bbb87a";
            string url = $"http://www.omdbapi.com/?apikey={apiKey}&t={movieTitle}"; // Example query

            var movieData = await GetMovieDataAsync(url);
            OmdbMovie movie = JsonConvert.DeserializeObject<OmdbMovie>(movieData.ToString());
            if (movie.Title != null)
            {
                return movie;
            }
            else
            {
                return null;
            }
        }


        public async Task<string> GetMovieDataAsync(string url)
        {
            try
            {
                var response = await client.SendAsync(new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(url),
                });

                response.EnsureSuccessStatusCode(); // Throw if not a success code.

                return await response.Content.ReadAsStringAsync();
                //return JObject.Parse(responseBody); // Parse the response body as JSON
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
                return null;
            }
        }

    }
}
