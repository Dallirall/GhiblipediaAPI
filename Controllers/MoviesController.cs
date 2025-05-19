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
        private readonly MovieRepository _movieRepo;

        public MoviesController(MovieRepository movieRepo)
        {
            _movieRepo = movieRepo;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Movie>> Get()
        {
            var movies = _movieRepo.GetAllMovies();
            return Ok(movies);
        }

        [HttpGet("{movie_id}")]
        public ActionResult<Movie> Get(int movie_id)
        {
            var movie = _movieRepo.GetMovieById(movie_id);

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }
    }
}
//[ProducesResponseType<T>(StatusCodes.Status200OK)]
//[ProducesResponseType(StatusCodes.Status404NotFound)]