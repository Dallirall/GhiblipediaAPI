using GhiblipediaAPI.Data;
using Microsoft.AspNetCore.Mvc;

namespace GhiblipediaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly IMovieRepository _movieRepo;

    public TestController(IMovieRepository movieRepo)
    {
        _movieRepo = movieRepo;
    }

    //[Route("api/test/{id:int?}")]
    //[Route("api/test/{title:string?}")]
    //public ActionResult GetMovieById(int id = 0, string title = "")
    //{
    //    var movie = _movieRepo.GetMovieById(id);

    //    if (movie == null) return NotFound();

    //    return Ok(movie);
    //}
}

