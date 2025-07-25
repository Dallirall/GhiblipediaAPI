using GhiblipediaAPI.Models;

namespace GhiblipediaAPI.Services
{
    public interface IOmdbService
    {
        //Fyll i, lägg till i Program.cs
        Task<OmdbMovie> GetOmdbMovie(string movieTitle);

        Task<string?> GetOmdbFullPlot(string movieTitle);

        Task<string> GetMovieDataAsync(string url);
    }
}
