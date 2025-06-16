using GhiblipediaAPI.Models;

namespace GhiblipediaAPI.Data
{
    public interface IMovieRepository
    {
        object GetTest();
        IEnumerable<Movie> GetAllMovies();

        Movie GetMovieByID(int id);

        public Movie GetMovieByTitle(string english_title);

        void PostMovieInDB(Movie movie);
                
    }
}
