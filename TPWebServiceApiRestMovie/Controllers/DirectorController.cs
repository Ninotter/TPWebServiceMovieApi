using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using TPWebServiceApiRestMovie.Context;
using TPWebServiceApiRestMovie.Models;

namespace TPWebServiceApiRestMovie.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DirectorController : ControllerBase
    {
        private readonly ApiContext _context;
        private JsonSerializerOptions options = new JsonSerializerOptions();
        public DirectorController(ApiContext context)
        {
            _context = context;
            options.MaxDepth = 0;
            options.ReferenceHandler = ReferenceHandler.Preserve;
        }

        /// <summary>
        /// Gets a specific director
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(Person))]
        [ProducesResponseType(404)]
        public JsonResult Get(int id)
        {
            var result = _context.Persons
                .Where(p => p.Id == id)
                .Include(p => p.MoviesPlayed)
                .Include(p => p.MoviesDirected)
                .First();
            ;

            if (result == null || !result.IsDirector)
            {
                Response.StatusCode = 404;
                return new JsonResult(NotFound());
            }

            Response.StatusCode = 200;
            return new JsonResult(Ok(result).Value, options);
        }
    }
}
