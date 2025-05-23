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

        [HttpGet]
        public ActionResult<IEnumerable<Movie>> Get()
        {
            var movies = _movieRepo.GetAllMovies();
            return Ok(movies);
        }

        [HttpGet("id={movie_id}")]
        public ActionResult<Movie> Get(int movie_id)
        {
            var movie = _movieRepo.GetMovieById(movie_id);

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }

        [HttpGet("title={english_title}")]
        public ActionResult<Movie> Get(string english_title)
        {
            var movie = _movieRepo.GetMovieByTitle(english_title);

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
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