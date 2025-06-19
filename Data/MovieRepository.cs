using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly OmdbAPIService _omdbAPI;

        public MovieRepository(IDbConnection db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _omdbAPI = new OmdbAPIService();
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
            if (movie.MovieId != null)
            {
                movie.MovieId = null; //This field auto-increment by default.
            }
            var movieDto = ConvertMovieToMovieDto(movie);
            string sqlQuery = CustomSqlServices.CreateInsertQueryStringFromObject(movieDto, "movies");

            _db.Execute(sqlQuery, movieDto);
        }

        public async Task<Movie> ConvertOmdbMovieToMovie(string englishTitle)
        {
            OmdbMovie omdbMovie = await _omdbAPI.GetOmdbMovie(englishTitle);

            Movie movie = _mapper.Map<Movie>(omdbMovie);

            return movie;

        }

        
    }
}
