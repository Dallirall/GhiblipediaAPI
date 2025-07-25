using GhiblipediaAPI.Models;

namespace GhiblipediaAPI.Services
{
    public interface IOmdbService
    {
        Task<OmdbMovie> GetOmdbMovie(string title);

        Task<string?> GetOmdbFullPlot(string title);

        Task<string> GetMovieDataAsync(string url);

        MovieInput ConvertOmdbMovieToMovieInput(OmdbMovie omdbMovie);
    }
}
