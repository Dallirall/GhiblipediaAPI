using GhiblipediaAPI.Models;

namespace GhiblipediaAPI.Data
{
    public interface IMovieRepository
    {
        IEnumerable<Movie> GetAllMovies();

        Movie GetMovieById(int id);

        public Movie GetMovieByTitle(string english_title);

        void PostMovieInDB(Movie movie);
                
    }
}
