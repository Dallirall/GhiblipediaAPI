using Dapper;
using GhiblipediaAPI.Models;
using GhiblipediaAPI.Services;
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

        public Movie GetMovieByTitle(string english_title)
        {
            string slqQuery = $"SELECT * FROM movies WHERE english_title = '{english_title}';";

            return _db.QueryFirstOrDefault<Movie>(slqQuery);
        }

        public void PostMovieInDB(Movie movie)
        {
            if (movie.Movie_id != null)
            {
                movie.Movie_id = null; //This field auto-increment by default.
            }

            string sqlQuery = CustomSqlServices.CreateInsertQueryStringFromObject(movie, "movies");

            _db.Execute(sqlQuery, movie);
        }
    }
}
