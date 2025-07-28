using GhiblipediaAPI.Models;

namespace GhiblipediaAPI.Data
{
    public interface IMovieRepository
    {
        Task<IEnumerable<MovieResponse>> GetAllMovies();

        public Task<MovieResponse> GetMovieByID(int id);

        public Task<MovieResponse> GetMovieByTitle(string title);

        public Task PostMovieInDB(MovieCreate movie);

        public Task UpdateMovieInDb(int? id, MovieUpdate movie);

    }
}
