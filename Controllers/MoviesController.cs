using Microsoft.AspNetCore.Mvc;

namespace GhiblipediaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase  
    {
        [HttpGet]

        //[ProducesResponseType<T>(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
