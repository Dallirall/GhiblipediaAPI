using GhiblipediaAPI.Models;

namespace GhiblipediaAPI.Data
{
    public interface IMovieRepository
    {
        Task<IEnumerable<MovieGet>> GetAllMovies();

        public Task<MovieGet> GetMovieByID(int id);

        public Task<MovieGet> GetMovieByTitle(string english_title);

        public Task<bool> PostMovieInDB(MoviePostPut movie);

        public Task<MoviePostPut> ConvertOmdbMovieToMoviePost(string englishTitle);

        public MoviePostPut ConvertMovieGetToMoviePost(MovieGet movieGet);

        public Task<int> UpdateMovieInDB(string englishTitle, MoviePostPut MovieDataToUpdate);//Ändra denna?

        public Task UpdateMovie(int? movieId, MoviePostPut movie);

    }
}
