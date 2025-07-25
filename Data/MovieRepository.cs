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
            string sqlQuery = $"SELECT * FROM movies WHERE movie_id = @movie_id;";

            try
            {
                var result = await _db.QueryFirstOrDefaultAsync<MovieResponse>(sqlQuery, new { movie_id = id });
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

        public async Task<bool> PostMovieInDB(MovieInput movie)
        {
            bool isSuccess = false;
            if (movie == null)
            {
                Console.WriteLine("Could not find the data to post in database.");
                return isSuccess;
            }    

            var existingMovie = await GetMovieByTitle(movie.EnglishTitle);

            if (existingMovie != null)
            {
                Console.WriteLine($"The movie '{movie.EnglishTitle}' already exists in database. ");
                return isSuccess;
            }

            string sqlQuery = CustomSqlServices.CreateInsertQueryStringFromDTO(movie, "movies");

            try
            {
                Console.WriteLine("Inserting into database... ");
                await _db.ExecuteAsync(sqlQuery, movie);
                isSuccess = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");                
            }
            return isSuccess; 
        }

        //Updates the specified movie in the database with the populated properties of the passed movieNewData object.
        public async Task UpdateMovieInDb(int? id, MovieInput movieNewData)
        {
            string updateQuery = CustomSqlServices.CreateUpdateQueryStringFromDTO(movieNewData, "movies", $"movie_id = {id}");

            int rowsUpdated = 0;
            rowsUpdated = await _db.ExecuteAsync(updateQuery, movieNewData);
        }
    }
}
