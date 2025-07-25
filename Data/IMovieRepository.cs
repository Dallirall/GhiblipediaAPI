using GhiblipediaAPI.Models;

namespace GhiblipediaAPI.Data
{
    public interface IMovieRepository
    {
        Task<IEnumerable<MovieResponse>> GetAllMovies();

        public Task<MovieResponse> GetMovieByID(int id);

        public Task<MovieResponse> GetMovieByTitle(string english_title);

        public Task<bool> PostMovieInDB(MovieInput movie);

        public Task<MovieInput> ConvertOmdbMovieToMovieInput(string englishTitle);

        public Task UpdateMovieInDb(int? movieId, MovieInput movie);

        Task<string?> GetFullPlot(string englishTitle);
    }
}
