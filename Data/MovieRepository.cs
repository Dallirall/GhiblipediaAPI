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


        public object GetTest()
        {
            string sqlQuery = "SELECT * FROM test_table;";

            var result = _db.Query(sqlQuery);
            return result;
        }


        public Movie ConvertMovieDtoToMovie(MovieDtoReadOnly dto)
        {
            return _mapper.Map<Movie>(dto);
        }

        public MovieDtoReadOnly ConvertMovieToMovieDto(Movie movie)
        {
            return _mapper.Map<MovieDtoReadOnly>(movie);
        }

        //Async??
        public IEnumerable<Movie> GetAllMovies()
        {
            string sqlQuery = "SELECT * FROM movies;";

            var result = _db.Query<MovieDtoReadOnly>(sqlQuery);
            if (result == null) return null; //Rätt..?

            return result.Select(dto => ConvertMovieDtoToMovie(dto));
        }

        public async Task<Movie> GetMovieByID(int id)
        {
            string sqlQuery = $"SELECT * FROM movies WHERE movie_id = @movie_id;";

            try
            {
                var result = await _db.QueryFirstOrDefaultAsync<MovieDtoReadOnly>(sqlQuery, new { movie_id = id });
                if (result == null) return null; //Rätt..?

                return ConvertMovieDtoToMovie(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Detailed error: " + ex.ToString());
                throw;
            }

        }

        public async Task<Movie> GetMovieByTitle(string englishTitle)
        {
            string sqlQuery = $"SELECT * FROM movies WHERE english_title = @english_title;";


            try
            {
                var result = await _db.QueryFirstOrDefaultAsync<MovieDtoReadOnly>(sqlQuery, new { english_title = englishTitle });
                if (result == null) return null; //Rätt..?

                return ConvertMovieDtoToMovie(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Detailed error: " + ex.ToString());
                throw;
            }
        }



        //Den här metoden hade kunnat vara bra att unit testa ev.

        public async Task<bool> PostMovieInDB(Movie movie)
        {
            bool isSuccess = false;
            if (movie == null)
            {
                Console.WriteLine("Could not find the data to post in database.");
                return isSuccess;
            }

            if (movie.MovieId != null)
            {
                movie.MovieId = null; //This field auto-increment by default.
            }
            if (movie.CreatedAt != null)
            {
                movie.CreatedAt = null; //This field will be timestamped in database automatically.
            }
            var movieDto = ConvertMovieToMovieDto(movie);

            var existingMovie = await GetMovieByTitle(movieDto.English_title);

            if (existingMovie != null)
            {
                Console.WriteLine($"The movie '{movieDto.English_title}' already exists in database. ");
                return isSuccess;
            }

            string sqlQuery = CustomSqlServices.CreateInsertQueryStringFromObject(movieDto, "movies");

            try
            {
                Console.WriteLine("Inserting into database... ");
                await _db.ExecuteAsync(sqlQuery, movieDto);
                isSuccess = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");                
            }
            return isSuccess; //Testa
        }

        public async Task<Movie> ConvertOmdbMovieToMovie(string englishTitle)
        {
            OmdbMovie omdbMovie = await _omdbAPI.GetOmdbMovie(englishTitle);

            Movie movie = _mapper.Map<Movie>(omdbMovie);

            return movie;

        }

        public async Task<int> UpdateMovieInDB(string englishTitle, Movie MovieNewData)
        {
            MovieDtoReadOnly movieDtoNewData = ConvertMovieToMovieDto(MovieNewData);

            PropertyInfo[] properties = movieDtoNewData.GetType()
                                            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                            .Where(prop => prop.GetValue(movieDtoNewData) != null).ToArray();
            
            int rowsUpdated = 0;
            foreach (var property in properties)
            {
                string updateQuery = $"UPDATE movies SET @Column = @Value WHERE english_title = @English_title;";

                var placeHolders = new { Value = property.GetValue(movieDtoNewData), English_title = englishTitle, Column = property.Name.ToLower() };
                                
                try
                {
                    Console.WriteLine($"Updating column: {property.Name.ToLower()} with value: {placeHolders.Value?.ToString() ?? "NULL"}");

                    rowsUpdated += await _db.ExecuteAsync(updateQuery, placeHolders);

                    Console.WriteLine("Currently updated rows: " + rowsUpdated);
                }
                catch (Exception ex)
                {                    
                    Console.WriteLine($"Could not update column {property.Name.ToLower()}. Exception: {ex.Message}");
                }
                                
            }
            return rowsUpdated;
        }

        public async Task UpdateMovie(Movie movieToUpdate)
        {
            string replaceQuery = "REPLACE INTO movies VALUES @Values;";

            int rowsUpdated = 0;
            rowsUpdated = await _db.ExecuteAsync(replaceQuery, movieToUpdate);
        }

    }
}
