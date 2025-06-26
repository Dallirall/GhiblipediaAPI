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


        public Movie ConvertMovieDtoToMovie(MovieDto dto)
        {
            return _mapper.Map<Movie>(dto);
        }

        public MovieDto ConvertMovieToMovieDto(Movie movie)
        {
            return _mapper.Map<MovieDto>(movie);
        }

        //Async??
        public IEnumerable<Movie> GetAllMovies()
        {
            string sqlQuery = "SELECT * FROM movies;";

            var result = _db.Query<MovieDto>(sqlQuery);
            if (result == null) return null; //Rätt..?

            return result.Select(dto => ConvertMovieDtoToMovie(dto));
        }

        public Movie GetMovieByID(int id)
        {
            string slqQuery = $"SELECT * FROM movies WHERE movie_id = {id};";

            var result = _db.QueryFirstOrDefault<MovieDto>(slqQuery);
            if (result == null) return null; //Rätt..?

            return ConvertMovieDtoToMovie(result);
        }

        public Movie GetMovieByTitle(string englishTitle)
        {
            string slqQuery = $"SELECT * FROM movies WHERE english_title = '{englishTitle}';";
            
            var result = _db.QueryFirstOrDefault<MovieDto>(slqQuery);
            if (result == null) return null; //Rätt..?

            return ConvertMovieDtoToMovie(result);
        }



        //Den här metoden hade kunnat vara bra att unit testa ev.

        public void PostMovieInDB(Movie movie)
        {
            if (movie == null)
            {
                throw new Exception("Could not find the data to post in database.");
            }

            if (movie.MovieId != null)
            {
                movie.MovieId = null; //This field auto-increment by default.
            }
            var movieDto = ConvertMovieToMovieDto(movie);
            string sqlQuery = CustomSqlServices.CreateInsertQueryStringFromObject(movieDto, "movies");

            try
            {
                _db.Execute(sqlQuery, movieDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
            //TODO: Returna något för att indikera om det failade, t.ex. om filmen redan finns i databasen..?
        }

        public async Task<Movie> ConvertOmdbMovieToMovie(string englishTitle)
        {
            OmdbMovie omdbMovie = await _omdbAPI.GetOmdbMovie(englishTitle);

            Movie movie = _mapper.Map<Movie>(omdbMovie);

            return movie;

        }

        public async Task<int> UpdateMovieInDB(string englishTitle, Movie MovieDataToUpdate)
        {
            MovieDto movieDto = ConvertMovieToMovieDto(MovieDataToUpdate);

            PropertyInfo[] properties = movieDto.GetType()
                                            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                            .Where(prop => prop.GetValue(movieDto) != null).ToArray();
            
            int rowsUpdated = 0;
            foreach (var property in properties)
            {
                string updateQuery = $"UPDATE movies SET {property.Name.ToLower()} = @Value WHERE english_title = @English_title;";

                var placeHolders = new { Value = property.GetValue(movieDto), English_title = englishTitle };
                                
                try
                {
                    Console.WriteLine($"Updating column: {property.Name} with value: {placeHolders.Value?.ToString() ?? "NULL"}");

                    rowsUpdated += await _db.ExecuteAsync(updateQuery, placeHolders);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                
            }
            return rowsUpdated;
        }



    }
}
