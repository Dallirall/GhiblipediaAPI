using GhiblipediaAPI.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace GhiblipediaAPI.Services
{
    //For transactions between this API and OMDb API
    public class OmdbService : IOmdbService
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly string _apiKey;

        public OmdbService(IOptions<OmdbAPIOptions> options)
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

        public async Task<string?> GetOmdbFullPlot(string movieTitle)
        {
            string url = $"http://www.omdbapi.com/?apikey={_apiKey}&t={movieTitle}&plot=full";
            
            var movieData = await GetMovieDataAsync(url);
            OmdbMovie movie = JsonConvert.DeserializeObject<OmdbMovie>(movieData.ToString());
            if (movie.Title != null)
            {
                return movie.Plot;
            }
            else
            {
                return null;
            }
        }

        //Sends a GET request to OMDb API and returns the JSON response as a string.
        public async Task<string> GetMovieDataAsync(string url)
        {
            try
            {
                var response = await client.SendAsync(new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(url),
                });

                response.EnsureSuccessStatusCode();

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
