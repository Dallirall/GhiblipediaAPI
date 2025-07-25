using GhiblipediaAPI.Models;

namespace GhiblipediaAPI.Data
{
    public interface IMovieRepository
    {
        Task<IEnumerable<MovieResponse>> GetAllMovies();

        public Task<MovieResponse> GetMovieByID(int id);

        public Task<MovieResponse> GetMovieByTitle(string english_title);

        public Task<bool> PostMovieInDB(MoviePostPut movie);

        public Task<MoviePostPut> ConvertOmdbMovieToMoviePost(string englishTitle);

        public Task UpdateMovieInDb(int? movieId, MoviePostPut movie);

        Task<string?> GetFullPlot(string englishTitle);
    }
}
