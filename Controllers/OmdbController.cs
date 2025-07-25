using GhiblipediaAPI.Data;
using GhiblipediaAPI.Models;
using GhiblipediaAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GhiblipediaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OmdbController : ControllerBase
    {
        private readonly IOmdbService _omdbService;
        private readonly IMovieRepository _movieRepo;

        public OmdbController(IOmdbService omdbService, IMovieRepository movieRepo)
        {
            _omdbService = omdbService;
            _movieRepo = movieRepo;
        }

        [HttpGet]
        [Route("{title}")]
        public async Task<ActionResult<OmdbMovie>> GetByTitle(string title)
        {
            if (title == null) return BadRequest();
            try
            {
                OmdbMovie movie = await _omdbService.GetOmdbMovie(title);

                if (movie == null) return NotFound();

                movie.FullPlot = await _omdbService.GetOmdbFullPlot(title);

                return Ok(movie);
            }
            catch (HttpRequestException ex) //ToDo: Check possible exceptions
            {
                return BadRequest(ex.Message); //Correct status code??
            }
        }

        [HttpPost]
        [Route("{title}")]
        public async Task<IActionResult> PostMovieInDBWithDataFromOmdb(string title)
        {
            if (title == null) return BadRequest();

            MovieInput movie = new MovieInput();
            movie = await _movieRepo.ConvertOmdbMovieToMovieInput(title); //Gets movie data from OMDb API and converts to movie object.

            bool isSuccess = await _movieRepo.PostMovieInDB(movie);

            if (!isSuccess)
            {
                return StatusCode(500, "Internal server error");
            }

            return CreatedAtAction(nameof(GetAll), movie);
        }


    }
}
