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
            catch (HttpRequestException ex)
            {
                return StatusCode(502, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
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

                MovieCreate movie = _omdbService.ConvertOmdbMovieToMovieCreate(omdbMovie);
                                
                await _movieRepo.PostMovieInDB(movie);

                return Created(); //ToDo: Fix proper return for frontend

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


    }
}
