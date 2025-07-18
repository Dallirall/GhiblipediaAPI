﻿using GhiblipediaAPI.Data;
using GhiblipediaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;

namespace GhiblipediaAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/")] //Sets the default route for API call URLs to /api/Movies/
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _movieRepo;

        public MoviesController(IMovieRepository movieRepo)
        {
            _movieRepo = movieRepo;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IEnumerable<MovieGet>>> GetAll()
        {
            var movies = await _movieRepo.GetAllMovies();
            if (movies == null) return NotFound();

            return Ok(movies);
        }

        [HttpGet]
        [Route("{movieID:int}")]        
        public async Task<ActionResult<MovieGet>> GetByID(int movieID)
        {
            try
            {
                var movie = await _movieRepo.GetMovieByID(movieID);

                if (movie == null) return NotFound();

                return Ok(movie);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Controller caught: " + ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }
                
        [HttpGet]
        [Route("{englishTitle}")]        
        public async Task<ActionResult<MovieGet>> GetByTitle(string englishTitle)
        {
            try
            {
                var movie = await _movieRepo.GetMovieByTitle(englishTitle);

                if (movie == null) return NotFound();

                return Ok(movie);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Controller caught: " + ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }

        //Insert a movie object with data from the JSON body into database. Use when you want to assign your own values to the object properties.
        [HttpPost]
        [Route("")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> PostMovie([FromBody] MoviePostPut movie)
        {
            if (movie == null) return UnprocessableEntity(); 
            bool isSuccess = await _movieRepo.PostMovieInDB(movie);
            
            if (!isSuccess)
            {
                return StatusCode(500, "Internal server error");
            }

            return CreatedAtAction(nameof(GetAll), movie);
        }

        //Searches OMDb API for the specified movie. Inserts that movie into database, assigning the retrieved data from OMDb to the corresponding database columns. 
        [HttpPost]
        [Route("omdb/{englishTitle}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> PostMovieInDBWithDataFromOmdb(string englishTitle)
        {
            if (englishTitle == null) return UnprocessableEntity();

            MoviePostPut movie = new MoviePostPut();
            movie = await _movieRepo.ConvertOmdbMovieToMoviePost(englishTitle); //Gets movie data from OMDb API and converts to movie object.

            bool isSuccess = await _movieRepo.PostMovieInDB(movie);

            if (!isSuccess)
            {
                return StatusCode(500, "Internal server error");
            }

            return CreatedAtAction(nameof(GetAll), movie);
        }

        //Update a movie in database. Caller can omit fields in the request body if those should not be updated.
        [HttpPut]
        [Route("{englishTitle}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateMovieByTitle(string englishTitle, [FromBody] MoviePostPut MovieNewData)
        {
            if (MovieNewData == null) return UnprocessableEntity();

            try
            {
                MovieGet movieFromDb = await _movieRepo.GetMovieByTitle(englishTitle);
                if (movieFromDb == null)
                {
                    Console.WriteLine($"The movie {englishTitle} does not yet exist in database. ");
                    return BadRequest();
                }

                await _movieRepo.UpdateMovieInDb(movieFromDb.MovieId, MovieNewData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

            return Ok();
        }

        //Update a movie in database. Caller can omit fields in the request body if those should not be updated.
        [HttpPut]
        [Route("{movieID:int}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateMovieById(int movieID, [FromBody] MoviePostPut MovieNewData)
        {
            if (MovieNewData == null) return UnprocessableEntity();

            try
            {
                MovieGet movieFromDb = await _movieRepo.GetMovieByID(movieID);
                if (movieFromDb == null)
                {
                    Console.WriteLine($"The movie does not yet exist in database. ");
                    return BadRequest();
                }

                await _movieRepo.UpdateMovieInDb(movieFromDb.MovieId, MovieNewData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

            return Ok();
        }

        //Probably unnecessary method. Delete in future if not needed.
        //Updates a movie in the database using JSON Patch. (Fetches the movie from database, applies the operation specified in patchDoc and updated it in database).
        [HttpPatch]
        [Route("{englishTitle}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> PatchMovie(string englishTitle, [FromBody] JsonPatchDocument<MovieGet> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();


            var movieFromDb = await _movieRepo.GetMovieByTitle(englishTitle);
            if (movieFromDb == null)
                return NotFound();
                        
            patchDoc.ApplyTo(movieFromDb, ModelState);
            
            var updateMovie = _movieRepo.ConvertMovieGetToMoviePost(movieFromDb);

            await _movieRepo.UpdateMovieInDb(movieFromDb.MovieId, updateMovie);
            return Ok(movieFromDb);

        }

        //Possibly unnecessary method. Delete in future if not needed.
        //For updating the 'plot' field of an existing object in the database with the full plot parameter from OMDb.
        [HttpPut]
        [Route("omdbPlot/{englishTitle}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdatePlotFromOmbd(string englishTitle)
        {
            if (englishTitle == null) return UnprocessableEntity();
            
            MovieGet movieFromDb = await _movieRepo.GetMovieByTitle(englishTitle);
            if (movieFromDb == null)
            {
                Console.WriteLine($"The movie {englishTitle} does not yet exist in database. ");
                return BadRequest();
            }

            MoviePostPut movie = new MoviePostPut();

            movie.Plot = await _movieRepo.GetFullPlot(englishTitle);

            return await UpdateMovieByTitle(englishTitle, movie);
        }

    }
}