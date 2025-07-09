using AutoMapper;
using Dapper;
using GhiblipediaAPI.Models;
using GhiblipediaAPI.Services;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Transactions;

namespace GhiblipediaAPI.Data
{
    public class MovieRepository : IMovieRepository
    {
        private readonly IDbConnection _db;
        private readonly IMapper _mapper;
        private readonly OmdbAPIService _omdbAPI;

        public MovieRepository(IDbConnection db, IMapper mapper, OmdbAPIService omdbAPI)
        {
            _db = db;
            _mapper = mapper;
            _omdbAPI = omdbAPI;
        }

        public MovieGet ConvertMovieDtoToMovieGet(MovieDtoGet dto)
        {
            return _mapper.Map<MovieGet>(dto);
        }

        public MovieDtoGet ConvertMovieGetToMovieDto(MovieGet movie)
        {
            return _mapper.Map<MovieDtoGet>(movie);
        }

        public MoviePostPut ConvertMovieDtoPostToMoviePost(MovieDtoPostPut dto)
        {
            return _mapper.Map<MoviePostPut>(dto);
        }

        public MovieDtoPostPut ConvertMoviePostToMovieDtoPost(MoviePostPut dto)
        {
            return _mapper.Map<MovieDtoPostPut>(dto);
        }

        public MoviePostPut ConvertMovieGetToMoviePost(MovieGet movieGet)
        {
            return _mapper.Map<MoviePostPut>(movieGet);
        }

        public async Task<IEnumerable<MovieGet>> GetAllMovies()
        {
            string sqlQuery = "SELECT * FROM movies;";

            var result = await _db.QueryAsync<MovieDtoGet>(sqlQuery);
            if (result == null) return null;

            return result.Select(dto => ConvertMovieDtoToMovieGet(dto));
        }

        public async Task<MovieGet> GetMovieByID(int id)
        {
            string sqlQuery = $"SELECT * FROM movies WHERE movie_id = @movie_id;";

            try
            {
                var result = await _db.QueryFirstOrDefaultAsync<MovieDtoGet>(sqlQuery, new { movie_id = id });
                if (result == null) return null;

                return ConvertMovieDtoToMovieGet(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Detailed error: " + ex.ToString());
                throw;
            }

        }

        public async Task<MovieGet> GetMovieByTitle(string englishTitle)
        {
            string sqlQuery = $"SELECT * FROM movies WHERE english_title = @english_title;";


            try
            {
                var result = await _db.QueryFirstOrDefaultAsync<MovieDtoGet>(sqlQuery, new { english_title = englishTitle });
                if (result == null) return null;

                return ConvertMovieDtoToMovieGet(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Detailed error: " + ex.ToString());
                throw;
            }
        }



        //Den här metoden hade kunnat vara bra att unit testa ev.

        public async Task<bool> PostMovieInDB(MoviePostPut movie)
        {
            bool isSuccess = false;
            if (movie == null)
            {
                Console.WriteLine("Could not find the data to post in database.");
                return isSuccess;
            }
            
            var movieDtoPost = ConvertMoviePostToMovieDtoPost(movie);

            var existingMovie = await GetMovieByTitle(movieDtoPost.English_title);

            if (existingMovie != null)
            {
                Console.WriteLine($"The movie '{movieDtoPost.English_title}' already exists in database. ");
                return isSuccess;
            }

            string sqlQuery = CustomSqlServices.CreateInsertQueryStringFromObject(movieDtoPost, "movies");

            try
            {
                Console.WriteLine("Inserting into database... ");
                await _db.ExecuteAsync(sqlQuery, movieDtoPost);
                isSuccess = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");                
            }
            return isSuccess; 
        }

        public async Task<MoviePostPut> ConvertOmdbMovieToMoviePost(string englishTitle)
        {
            OmdbMovie omdbMovie = await _omdbAPI.GetOmdbMovie(englishTitle);

            if (omdbMovie != null)
            {
                MoviePostPut movie = _mapper.Map<MoviePostPut>(omdbMovie);
                return movie;
            }

            return null;

        }

        //public async Task<int> UpdateMovie(int? movieId, MoviePostPut MovieNewData)
        //{
        //    MovieDtoPostPut movieDtoNewData = ConvertMoviePostToMovieDtoPost(MovieNewData);

        //    return 1;

        //    //PropertyInfo[] properties = movieDtoNewData.GetType()
        //    //                                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
        //    //                                .Where(prop => prop.GetValue(movieDtoNewData) != null).ToArray();
            
        //    //int rowsUpdated = 0;
        //    //foreach (var property in properties)
        //    //{
        //    //    string updateQuery = $"UPDATE movies SET @Column = @Value WHERE english_title = @English_title;";

        //    //    var placeHolders = new { Value = property.GetValue(movieDtoNewData), English_title = englishTitle, Column = property.Name.ToLower() };
                                
        //    //    try
        //    //    {
        //    //        Console.WriteLine($"Updating column: {property.Name.ToLower()} with value: {placeHolders.Value?.ToString() ?? "NULL"}");

        //    //        rowsUpdated += await _db.ExecuteAsync(updateQuery, placeHolders);

        //    //        Console.WriteLine("Currently updated rows: " + rowsUpdated);
        //    //    }
        //    //    catch (Exception ex)
        //    //    {                    
        //    //        Console.WriteLine($"Could not update column {property.Name.ToLower()}. Exception: {ex.Message}");
        //    //    }
                                
        //    //}
        //    //return rowsUpdated;
        //}
                
        public async Task UpdateMovieInDb(int? movieId, MoviePostPut MovieNewData)
        {
            MovieDtoPostPut movieDtoNewData = ConvertMoviePostToMovieDtoPost(MovieNewData);

            string updateQuery = CustomSqlServices.CreateUpdateQueryStringFromObject(movieDtoNewData, "movies", $"movie_id = {movieId}");

            int rowsUpdated = 0;
            rowsUpdated = await _db.ExecuteAsync(updateQuery, movieDtoNewData);
        }

    }
}
