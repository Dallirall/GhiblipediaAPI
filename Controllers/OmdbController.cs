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

            try
            {
                OmdbMovie omdbMovie = await _omdbService.GetOmdbMovie(title);

                if (omdbMovie == null) return NotFound(); //Overkill?

                MovieInput movie = _omdbService.ConvertOmdbMovieToMovieInput(omdbMovie);

                if (movie == null) throw new Exception("Failed to convert OMDb data. ");

                bool isSuccess = await _movieRepo.PostMovieInDB(movie);

                if (!isSuccess) throw new Exception("Failed insert into database. ");

                return CreatedAtRoute(RedirectToAction("GetByTitle", "Movies", movie.EnglishTitle), movie); //Funkar..?
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message); //Kan man göra såhär? (Om inte ovan exc är throwad utan något annat..)
            }
        }


    }
}
