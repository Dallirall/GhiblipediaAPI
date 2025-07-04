using GhiblipediaAPI.Data;
using GhiblipediaAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks; //Behövs denna?

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


        [HttpGet]
        [Route("{testVar}/{moreTest}/{testInt:int}")]
        public ActionResult GetTestObj(string testVar, string moreTest, int testInt)
        {
            var testObj = _movieRepo.GetTest();

            return Ok(testObj);
        }



        //TODO: Gör det async

        //api/movies (ex: GET api/movies)
        [HttpGet]
        [Route("")]
        public ActionResult<IEnumerable<Movie>> GetAll()
        {
            var movies = _movieRepo.GetAllMovies();
            if (movies == null) return NotFound();

            return Ok(movies);
        }

        //api/movies/{movieID} (ex: GET api/movies/1) 
        [HttpGet]
        [Route("{movieID:int}")]        
        public async Task<ActionResult<Movie>> GetByID(int movieID)
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
        public async Task<ActionResult<Movie>> GetByTitle(string englishTitle)
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


        //api/movies/{englishTitle}/fullplot || api/movies/{englishTitle}/summary (ex: GET api/movies/spirited%20away/fullplot)
        [HttpGet]
        [Route("{englishTitle}/{plotType}")]
        public async Task<ActionResult<Movie>> GetFullPlotOrSummary(string englishTitle, string plotType)
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
            return BadRequest(); //Rätt??
        }

        [HttpPost]
        [Route("")]
        public IActionResult PostMovie([FromBody] Movie movie)
        {
            if (movie == null) return UnprocessableEntity(); //Korrekt??
            _movieRepo.PostMovieInDB(movie);          

            return CreatedAtAction(nameof(GetAll), movie);
        }

        [HttpPost]
        [Route("omdb/{englishTitle}")] //Borde routen göras på annat sätt?
        public async Task<IActionResult> PostMovieInDBWithDataFromOmdb(string englishTitle)
        {
            if (englishTitle == null) return UnprocessableEntity();

            Movie movie = new Movie();
            movie = await _movieRepo.ConvertOmdbMovieToMovie(englishTitle);            
            
            _movieRepo.PostMovieInDB(movie);

            return CreatedAtAction(nameof(GetAll), movie);
        }

        [HttpPut]
        [Route("{englishTitle}")]
        public async Task<IActionResult> UpdateMovie(string englishTitle, [FromBody] Movie MovieNewData)
        {
            if (MovieNewData == null) return UnprocessableEntity();

            try
            {
                Movie movieToUpdate = await _movieRepo.GetMovieByTitle(englishTitle);
                if (movieToUpdate == null)
                {
                    Console.WriteLine($"The movie {englishTitle} does not yet exist in database. ");
                    return BadRequest();
                }

                int rowsUpdatedResponse = await _movieRepo.UpdateMovieInDB(englishTitle, MovieNewData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

            return Ok((GetByTitle($"{englishTitle}")));
        }

        [HttpPatch]
        [Route("{englishTitle}")]
        public async Task<IActionResult> PatchMovie(string englishTitle, [FromBody] JsonPatchDocument<Movie> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();
            

            var movieToUpdate = await _movieRepo.GetMovieByTitle(englishTitle);
            if (movieToUpdate == null)
                return NotFound();

            //Kolla vadfan ModelState är...
            patchDoc.ApplyTo(movieToUpdate, ModelState);

            await _movieRepo.UpdateMovie(movieToUpdate);
            return Ok(movieToUpdate);

        }

    }
}
//[ProducesResponseType<T>(StatusCodes.Status200OK)]
//[ProducesResponseType(StatusCodes.Status404NotFound)]

//Felhantering??
//[Route("{englishTitle}/{fields}")]
//public IActionResult GetMovieSpecificFields(string englishTitle, string fields)
//{
//    var movie = _movieRepo.GetMovieByTitle(englishTitle);
//    if (movie == null) return NotFound();

//    var fieldList = fields?.Split(',', StringSplitOptions.RemoveEmptyEntries);
//    if (fieldList == null || fieldList.Length == 0)
//        return Ok(movie);

//    var result = new Dictionary<string, object>();
//    foreach (var field in fieldList)
//    {
//        switch (field.Trim().ToLower())
//        {
//            case "movie_id":
//                result["movie_id"] = movie.MovieId; break;
//            case "english_title":
//                result["english_title"] = movie.EnglishTitle; break;
//            case "japanese_title":
//                result["japanese_title"] = movie.JapaneseTitle; break;
//            case "release_year":
//                result["release_year"] = movie.ReleaseYear; break;
//            case "image_url":
//                result["image_url"] = movie.ImageUrl; break;
//            case "trailer_url":
//                result["trailer_url"] = movie.TrailerUrl; break;
//            case "summary":
//                result["summary"] = movie.Summary; break;
//            case "plot":
//                result["plot"] = movie.Plot; break;
//            case "director":
//                result["director"] = movie.Director; break;
//            case "genre":
//                result["genre"] = movie.Genre; break;
//            case "running_time_mins":
//                result["running_time_mins"] = movie.RunningTimeMins; break;
//            default: break;
//        }
//    }

//    return Ok(result);
//}
