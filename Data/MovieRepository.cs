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
        private readonly IMapper _mapper;
        private readonly OmdbAPIService _omdbAPI;

        public MovieRepository(IDbConnection db, IMapper mapper, OmdbAPIService omdbAPI)
        {
            _db = db;
            _mapper = mapper;
            _omdbAPI = omdbAPI;
        }

        //These convert methods uses Automapper to convert between the data model classes and the DTO classes
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
            string sqlQuery = $"SELECT * FROM movies WHERE LOWER(english_title) = LOWER(@english_title);";

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

        //Fetches a movie called {englishTitle} from OMDb API and maps the retrieved data to a MoviePostPut object.
        public async Task<MoviePostPut> ConvertOmdbMovieToMoviePost(string englishTitle)
        {
            OmdbMovie omdbMovie = await _omdbAPI.GetOmdbMovie(englishTitle);

            omdbMovie.FullPlot = await _omdbAPI.GetOmdbFullPlot(englishTitle);

            if (omdbMovie != null)
            {
                MoviePostPut movie = _mapper.Map<MoviePostPut>(omdbMovie);
                return movie;
            }

            return null;
        }

        //Updates the specified movie in the database with the populated properties of the passed MovieNewData object.
        public async Task UpdateMovieInDb(int? movieId, MoviePostPut MovieNewData)
        {
            MovieDtoPostPut movieDtoNewData = ConvertMoviePostToMovieDtoPost(MovieNewData);

            string updateQuery = CustomSqlServices.CreateUpdateQueryStringFromObject(movieDtoNewData, "movies", $"movie_id = {movieId}");

            int rowsUpdated = 0;
            rowsUpdated = await _db.ExecuteAsync(updateQuery, movieDtoNewData);
        }

        //Fetches the full plot data of a movie from OMDb API.
        public async Task<string?> GetFullPlot(string englishTitle)
        {
            return await _omdbAPI.GetOmdbFullPlot(englishTitle);
        }
    }
}
