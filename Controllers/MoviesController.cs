using Asp.Versioning;
using GhiblipediaAPI.Data;
using GhiblipediaAPI.Models;
using GhiblipediaAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace GhiblipediaAPI.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]/")]
    [ApiVersion("1.0")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _movieRepo;

        public MoviesController(IMovieRepository movieRepo)
        {
            _movieRepo = movieRepo;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IEnumerable<MovieResponse>>> GetAll()
        {            
            try
            {
                var movies = await _movieRepo.GetAllMovies();
                if (movies == null) return NotFound();

                return Ok(movies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("{id:int}")]        
        public async Task<ActionResult<MovieResponse>> GetByID(int id)
        {
            try
            {
                var movie = await _movieRepo.GetMovieByID(id);

                if (movie == null) return NotFound();

                return Ok(movie);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
                
        [HttpGet]
        [Route("{englishTitle}")]
        public async Task<ActionResult<MovieResponse>> GetByTitle(string englishTitle)
        {
            try
            {
                var movie = await _movieRepo.GetMovieByTitle(englishTitle);

                if (movie == null) return NotFound();

                return Ok(movie);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //Insert a movie object with data from the JSON body into database. Use when you want to assign your own values to the object properties.
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> PostMovie([FromBody] MovieCreate movie)
        {
            if (movie == null) return BadRequest();

            try
            {
                await _movieRepo.PostMovieInDB(movie);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return CreatedAtAction(nameof(GetAll), movie);
        }

        //Update a movie in database. Caller can omit fields in the request body if those should not be updated.
        [HttpPut]
        [Route("{englishTitle}")]
        public async Task<IActionResult> UpdateMovieByTitle(string englishTitle, [FromBody] MovieUpdate MovieNewData)
        {
            if (MovieNewData == null) return BadRequest();

            try
            {
                MovieResponse movieFromDb = await _movieRepo.GetMovieByTitle(englishTitle);
                if (movieFromDb == null)
                {
                    throw new Exception($"The movie {englishTitle} does not yet exist in database. ");
                }

                await _movieRepo.UpdateMovieInDb(movieFromDb.Id, MovieNewData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok();
        }

        //Update a movie in database. Caller can omit fields in the request body if those should not be updated.
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateMovieById(int id, [FromBody] MovieUpdate MovieNewData)
        {
            if (MovieNewData == null) return BadRequest();

            try
            {
                MovieResponse movieFromDb = await _movieRepo.GetMovieByID(id);
                if (movieFromDb == null)
                {
                    throw new Exception($"The movie with ID {id} does not yet exist in database. ");
                }

                await _movieRepo.UpdateMovieInDb(movieFromDb.Id, MovieNewData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok();
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteMovieById(int id)
        {            
            try
            {
                MovieResponse movieFromDb = await _movieRepo.GetMovieByID(id);
                if (movieFromDb == null)
                {
                    throw new Exception($"The movie with ID {id} does not exist in database. ");
                }

                await _movieRepo.DeleteMovie(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok();
        }
                
        [HttpDelete]
        [Route("{englishTitle}")]
        public async Task<IActionResult> DeleteMovieByTitle(string englishTitle)
        {
            if (englishTitle == null) return BadRequest();

            try
            {
                MovieResponse movieFromDb = await _movieRepo.GetMovieByTitle(englishTitle);
                if (movieFromDb == null)
                {
                    throw new Exception($"The movie '{englishTitle}' does not exist in database. ");
                }

                await _movieRepo.DeleteMovie(movieFromDb.Id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok();
        }


    }
}