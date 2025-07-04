using GhiblipediaAPI.Models;

namespace GhiblipediaAPI.Data
{
    public interface IMovieRepository
    {
        object GetTest();
        IEnumerable<Movie> GetAllMovies();

        public Task<Movie> GetMovieByID(int id);

        public Task<Movie> GetMovieByTitle(string english_title);

        public Task<bool> PostMovieInDB(Movie movie);

        public Task<Movie> ConvertOmdbMovieToMovie(string englishTitle);

        public Task<int> UpdateMovieInDB(string englishTitle, Movie MovieDataToUpdate);//Ändra denna?

        public Task UpdateMovie(Movie movie);

    }
}
