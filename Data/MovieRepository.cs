using AutoMapper;
using Dapper;
using GhiblipediaAPI.Models;
using GhiblipediaAPI.Services;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Transactions;
using System.Linq;

namespace GhiblipediaAPI.Data
{
    //Class for all business logic on the database's 'movies' table.
    public class MovieRepository : IMovieRepository
    {
        private readonly IDbConnection _db;
        private readonly IOmdbService _omdbAPI;

        public MovieRepository(IDbConnection db, IOmdbService omdbAPI)
        {
            _db = db;
            _omdbAPI = omdbAPI;
        }

        public async Task<IEnumerable<MovieResponse>> GetAllMovies()
        {
            string sqlQuery = "SELECT * FROM movies;";

            var result = await _db.QueryAsync<MovieResponse>(sqlQuery);
            if (result == null) return null;

            return result;
        }

        public async Task<MovieResponse> GetMovieByID(int id)
        {
            string sqlQuery = $"SELECT * FROM movies WHERE id = @Id;";

            try
            {
                var result = await _db.QueryFirstOrDefaultAsync<MovieResponse>(sqlQuery, new { Id = id });
                if (result == null) return null;

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Detailed error: " + ex.ToString());
                throw;
            }

        }

        public async Task<MovieResponse> GetMovieByTitle(string englishTitle)
        {
            //Leta först i engtitle sen japtitle
            string sqlQuery = $"SELECT * FROM movies WHERE LOWER(english_title) = LOWER(@english_title);";

            try
            {
                var result = await _db.QueryFirstOrDefaultAsync<MovieResponse>(sqlQuery, new { english_title = englishTitle });
                if (result == null) return null;

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Detailed error: " + ex.ToString());
                throw;
            }
        }

        public async Task PostMovieInDB(MovieCreate movie)
        {
            if (movie == null) throw new ArgumentNullException(nameof(movie), "Could not find the data to post in database");

            if (movie.EnglishTitle == null) throw new ArgumentException("The English title of the movie is required. ");

            var existingMovie = await GetMovieByTitle(movie.EnglishTitle);

            if (existingMovie != null) throw new Exception($"The movie '{movie.EnglishTitle}' already exists in database. ");

            string sqlQuery = CustomSqlServices.CreateInsertQueryStringFromDTO(movie, "movies");

            await _db.ExecuteAsync(sqlQuery, movie);

        }

        //Updates the specified movie in the database with the populated properties of the passed movieNewData object.
        public async Task UpdateMovieInDb(int id, MovieUpdate movieNewData)
        {
            //Onödigt?
            if (movieNewData == null) throw new ArgumentNullException(nameof(movieNewData), "Could not find data to update. ");
            
            string updateQuery = CustomSqlServices.CreateUpdateQueryStringFromDTO(movieNewData, "movies", $"id = {id}");

            int rowsUpdated = 0;
            rowsUpdated = await _db.ExecuteAsync(updateQuery, movieNewData);
            
        }
        public async Task DeleteMovie(int id)
        {
            string sqlQuery = $"DELETE FROM movies WHERE id = @Id;";
            int rowsUpdated = 0;
                        
            rowsUpdated = await _db.ExecuteAsync(sqlQuery, new { Id = id });

            if (rowsUpdated == 0)
            {
                throw new Exception($"The delete operation on the object with ID {id} was unsuccessful. No row in database with the ID could be found. ");
            }            
        }


    }
}
