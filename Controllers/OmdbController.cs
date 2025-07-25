using GhiblipediaAPI.Data;
using GhiblipediaAPI.Models;
using GhiblipediaAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GhiblipediaAPI.Controllers
{
    //For transactions with OMDb API
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

        //Searches OMDb API for the specified movie. Inserts that movie into database, assigning the retrieved data from OMDb to the corresponding database columns. 
        [HttpPost]
        [Route("{title}")]
        public async Task<IActionResult> PostMovieInDBWithDataFromOmdb(string title)
        {
            if (title == null) return BadRequest();

            try
            {
                OmdbMovie omdbMovie = await _omdbService.GetOmdbMovie(title);

                if (omdbMovie == null) return NotFound();

                MovieInput movie = _omdbService.ConvertOmdbMovieToMovieInput(omdbMovie);
                                
                await _movieRepo.PostMovieInDB(movie);
                
                return CreatedAtRoute(RedirectToAction("GetByTitle", "Movies", movie.EnglishTitle), movie); //Funkar..?
            }
            catch (ArgumentNullException a)
            {
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); //Kan man göra såhär? 
            }
        }


    }
}
