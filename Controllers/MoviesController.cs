using GhiblipediaAPI.Data;
using GhiblipediaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json; //Behövs denna?

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

        //api/movies/{movieId} (ex: GET api/movies/1) 
        [HttpGet]
        [Route("{movieId:int}")]        
        public ActionResult<Movie> GetById(int movieId)
        {
            var movie = _movieRepo.GetMovieById(movieId);

            if (movie == null) return NotFound();
            
            return Ok(movie);
        }

        //api/movies/{englishTitle} (ex: GET api/movies/spirited%20away)
        [HttpGet]
        [Route("{englishTitle}")]        
        public ActionResult<Movie> GetByTitle(string englishTitle)
        {
            var movie = _movieRepo.GetMovieByTitle(englishTitle);

            if (movie == null) return NotFound();

            return Ok(movie);
        }


        //api/movies/{englishTitle}/fullplot || api/movies/{englishTitle}/summary (ex: GET api/movies/spirited%20away/fullplot)
        [HttpGet]
        [Route("{englishTitle}/{plotType}")]
        public ActionResult<Movie> GetFullPlotOrSummary(string englishTitle, string plotType)
        {
            var movie = _movieRepo.GetMovieByTitle(englishTitle);

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

        [HttpPost]
        [Route("")]
        public IActionResult PostMovie([FromBody] Movie movie)
        {
            if (movie == null) return UnprocessableEntity(); //Korrekt??
            _movieRepo.PostMovieInDB(movie);          

            return CreatedAtAction(nameof(GetAll), movie);
        }
    }
}
//[ProducesResponseType<T>(StatusCodes.Status200OK)]
//[ProducesResponseType(StatusCodes.Status404NotFound)]