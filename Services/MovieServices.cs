using GhiblipediaAPI.Data;
using GhiblipediaAPI.Models;

namespace GhiblipediaAPI.Services
{
    public class MovieServices : IMovieServices
    {
        private readonly IMovieRepository _movieRepository;

        public MovieServices(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        // Hämta filmer till för listvy

        //public Movie GetMovieByTitle(string title) 
        //{
        //    // gå till db
        //}
        //public Movie GetMovieId(string id) 
        //{
        //    // gå till db
        //}

    }
}
