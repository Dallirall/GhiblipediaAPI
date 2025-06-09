using GhiblipediaAPI.Data;
using GhiblipediaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json; //Behövs denna?

namespace GhiblipediaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _movieRepo;

        public MoviesController(IMovieRepository movieRepo)
        {
            _movieRepo = movieRepo;
        }

        //TODO: Gör det async

        //api/movies (ex: GET api/movies)
        [HttpGet]
        [Route("")]
        public ActionResult<IEnumerable<Movie>> GetAll()
        {
            var movies = _movieRepo.GetAllMovies();
            return Ok(movies);
        }

        //api/movies/{movieId} (ex: GET api/movies/1)
        [HttpGet]
        [Route("{movie_id:int}")]        
        public ActionResult<Movie> GetById(int movie_id)
        {
            var movie = _movieRepo.GetMovieById(movie_id);

            if (movie == null) return NotFound();
            
            return Ok(movie);
        }

        //api/movies/{movietitle} (ex: GET api/movies/spirited%20away)
        [HttpGet]
        [Route("{english_title}")]        
        public ActionResult<Movie> GetByTitle(string english_title)
        {
            var movie = _movieRepo.GetMovieByTitle(english_title);

            if (movie == null) return NotFound();

            return Ok(movie);
        }


        //api/movies/{movietitle}/fullplot || api/movies/{movietitle}/summary (ex: GET api/movies/spirited%20away/fullplot)
        [HttpGet]
        [Route("{english_title}/{plotType}")]
        public ActionResult<Movie> GetFullPlotOrSummary(string english_title, string plotType)
        {
            var movie = _movieRepo.GetMovieByTitle(english_title);

            if (movie == null) return NotFound();

            if (plotType.ToLower() == "fullplot")
            {
                return Ok(movie.Plot);
            }
            else if (plotType.ToLower() == "summary")
            {
                return Ok(movie.Summary);
            }
            return BadRequest(); //Rätt??
        }

        //Felhantering??
        [Route("{english_title}/{fields}")]
        public IActionResult GetMovieSpecificFields(string english_title, string fields)
        {
            var movie = _movieRepo.GetMovieByTitle(english_title);
            if (movie == null) return NotFound();

            var fieldList = fields?.Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (fieldList == null || fieldList.Length == 0)
                return Ok(movie);

            var result = new Dictionary<string, object>();
            foreach (var field in fieldList)
            {
                switch (field.Trim().ToLower())
                {
                    case "movie_id":
                        result["movie_id"] = movie.Movie_id; break;
                    case "english_title":
                        result["english_title"] = movie.English_title; break;
                    case "japanese_title":
                        result["japanese_title"] = movie.Japanese_title; break;
                    case "release_year":
                        result["release_year"] = movie.Release_year; break;
                    case "image_url":
                        result["image_url"] = movie.Image_url; break;
                    case "trailer_url":
                        result["trailer_url"] = movie.Trailer_url; break;
                    case "summary":
                        result["summary"] = movie.Summary; break;
                    case "plot":
                        result["plot"] = movie.Plot; break;
                    case "director":
                        result["director"] = movie.Director; break;
                    case "genre":
                        result["genre"] = movie.Genre; break;
                    case "running_time_mins":
                        result["running_time_mins"] = movie.Running_time_mins; break;
                    default: break;
                }
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("")]
        public IActionResult Post([FromBody] Movie movie)
        {
            _movieRepo.PostMovieInDB(movie);

            return CreatedAtAction(nameof(GetAll), movie);
        }
    }
}
//[ProducesResponseType<T>(StatusCodes.Status200OK)]
//[ProducesResponseType(StatusCodes.Status404NotFound)]