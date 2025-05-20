using Dapper;
using GhiblipediaAPI.Models;
using System.Collections.Generic;
using System.Data;

namespace GhiblipediaAPI.Data
{
    public class MovieRepository : IMovieRepository
    {
        private readonly IDbConnection _db;

        public MovieRepository(IDbConnection db)
        {
            _db = db;
        }

        //Async??
        public IEnumerable<Movie> GetAllMovies()
        {
            string sqlQuery = "SELECT * FROM movies;";

            return _db.Query<Movie>(sqlQuery);
        }

        public Movie GetMovieById(int id)
        {
            string slqQuery = $"SELECT * FROM movies WHERE movie_id = {id};";

            return _db.QueryFirstOrDefault<Movie>(slqQuery);
        }

        public void PostMovieInDB(Movie movie)
        {

        }
    }
}
