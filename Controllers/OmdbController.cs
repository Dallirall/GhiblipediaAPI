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

        public OmdbController(IOmdbService omdbService)
        {
            _omdbService = omdbService;
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


    }
}
