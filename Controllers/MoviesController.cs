using GhiblipediaAPI.Data;
using GhiblipediaAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;

namespace GhiblipediaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _movieRepo;

        public MoviesController(IMovieRepository movieRepo)
        {
            _movieRepo = movieRepo;
        }

        //api/movies (ex: GET api/movies)
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IEnumerable<MovieGet>>> GetAll()
        {
            var movies = await _movieRepo.GetAllMovies();
            if (movies == null) return NotFound();

            return Ok(movies);
        }

        //api/movies/{movieID} (ex: GET api/movies/1) 
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

        //api/movies/{englishTitle} (ex: GET api/movies/spirited%20away)
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

        //Propably unnecessary method. Delete in future if not needed.
        //api/movies/{englishTitle}/fullplot OR api/movies/{englishTitle}/summary (ex: GET api/movies/spirited%20away/fullplot)
        [HttpGet]
        [Route("{englishTitle}/{plotType}")]
        public async Task<ActionResult<MovieGet>> GetFullPlotOrSummary(string englishTitle, string plotType)
        {
            var movie = await _movieRepo.GetMovieByTitle(englishTitle);

            if (movie == null) return NotFound();

            if (plotType.ToLower() == "fullplot")
            {
                return Ok(movie.Plot);
            }
            else if (plotType.ToLower() == "summary")
            {
                return Ok(movie.Summary);
            }
            return BadRequest(); 
        }

        [HttpPost]
        [Route("")]
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

        [HttpPost]
        [Route("omdb/{englishTitle}")]
        public async Task<IActionResult> PostMovieInDBWithDataFromOmdb(string englishTitle)
        {
            if (englishTitle == null) return UnprocessableEntity();

            MoviePostPut movie = new MoviePostPut();
            movie = await _movieRepo.ConvertOmdbMovieToMoviePost(englishTitle);

            bool isSuccess = await _movieRepo.PostMovieInDB(movie);

            if (!isSuccess)
            {
                return StatusCode(500, "Internal server error");
            }

            return CreatedAtAction(nameof(GetAll), movie);
        }

        [HttpPut]
        [Route("{englishTitle}")]
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

        [HttpPut]
        [Route("{movieID:int}")]
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

        //Possibly unnecessary method. Delete in future if not needed.
        [HttpPatch]
        [Route("{englishTitle}")]
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