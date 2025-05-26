using GhiblipediaAPI.Data;
using GhiblipediaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json; //Behövs denna?

namespace GhiblipediaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _movieRepo;

        public MoviesController(IMovieRepository movieRepo)
        {
            _movieRepo = movieRepo;
        }

        //TODO: Gör det async 
        [HttpGet]
        public ActionResult<IEnumerable<Movie>> Get()
        {
            var movies = _movieRepo.GetAllMovies();
            return Ok(movies);
        }

        [HttpGet("by-id")]
        public ActionResult<Movie> GetById([FromQuery] int movie_id)
        {
            var movie = _movieRepo.GetMovieById(movie_id);

            if (movie == null) return NotFound();
            
            return Ok(movie);
        }

        [HttpGet("by-title")]
        public ActionResult<Movie> GetByTitle([FromQuery] string english_title)
        {
            var movie = _movieRepo.GetMovieByTitle(english_title);

            if (movie == null) return NotFound();

            return Ok(movie);
        }

        //Felhantering??
        [HttpGet("by-title/partial")]
        public IActionResult GetMovieSpecificFields([FromQuery] string english_title, [FromQuery] string fields)
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
        public IActionResult Post([FromBody] Movie movie)
        {
            _movieRepo.PostMovieInDB(movie);

            return CreatedAtAction(nameof(Get), movie);
        }
    }
}
//[ProducesResponseType<T>(StatusCodes.Status200OK)]
//[ProducesResponseType(StatusCodes.Status404NotFound)]