using GhiblipediaAPI.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace GhiblipediaAPI.Services
{
    public class OmdbAPIService
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly string _apiKey;

        public OmdbAPIService(IOptions<OmdbAPIOptions> options)
        {
            _apiKey = options.Value.ApiKey
                ?? throw new InvalidOperationException("API key 'OmdbApi:ApiKey' is not configured.");
        }

        public async Task<OmdbMovie> GetOmdbMovie(string movieTitle)
        {            
            string url = $"http://www.omdbapi.com/?apikey={_apiKey}&t={movieTitle}";

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
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
                return null;
            }
        }

    }
}
