using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;
using TPWebServiceApiRestMovie.Context;
using TPWebServiceApiRestMovie.Models;

namespace TPWebServiceApiRestMovie.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly ApiContext _context;
        private JsonSerializerOptions options = new JsonSerializerOptions();
        public MovieController(ApiContext context)
        {
            _context = context;
            options.MaxDepth = 0;
            options.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        }

        /// <summary>
        /// Creates a movie
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///        "title": "Fight Club",
        ///        "description": "an anonymous narrator finds escape from his hollow life through an underground fighting club where men find their true selves through shared pain",
        ///        "releaseDate" : "1999-10-27T22:19:02.649Z"
        ///     }
        ///
        /// </remarks>
        /// 
        [ProducesResponseType(201, Type = typeof(Movie))]
        [ProducesResponseType(404)]
        [Produces("application/json")]
        [HttpPost]
        public JsonResult Create(Movie movie)
        {
            //Generates a new ID if the user mistakenly specified one
            movie.Id = 0;
            _context.Movies.Add(movie);
            _context.SaveChanges();

            Response.StatusCode = 201;
            return new JsonResult(Ok(movie).Value, options);
        }

        /// <summary>
        /// Edits a movie
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///        "id" : 1,
        ///        "title": "Fight Club",
        ///        "description": "an anonymous narrator finds escape from his hollow life through an underground fighting club where men find their true selves through shared pain",
        ///        "releaseDate" : "1999-10-27T22:19:02.649Z"
        ///     }
        ///
        /// </remarks>
        [ProducesResponseType(200, Type = typeof(Movie))]
        [ProducesResponseType(404)]
        [Produces("application/json")]
        [HttpPatch]
        public JsonResult Edit(Movie movie)
        {
            var movieInDb = _context.Movies.Find(movie.Id);

            if(movieInDb == null) {
                Response.StatusCode = 404;
                return new JsonResult(NotFound());
            }

            _context.Entry(movieInDb).CurrentValues.SetValues(movie);
            _context.SaveChanges();

            Response.StatusCode = 200;
            return new JsonResult(Ok(movie).Value, options);
        }

        /// <summary>
        /// Gets a movie
        /// </summary>
        [ProducesResponseType(200, Type = typeof(Movie))]
        [ProducesResponseType(404)]
        [Produces("application/json")]
        [HttpGet]
        public JsonResult Get(int id)
        {
            var result = _context.Movies
                .Where(p => p.Id == id)
                .Include(p => p.Actors)
                .Include(p => p.Directors)
                .FirstOrDefault();

            if (result == null)
            {
                Response.StatusCode = 404;
                return new JsonResult(NotFound());
            }
            Response.StatusCode = 200;
            return new JsonResult(Ok(result).Value, options);
        }

        /// <summary>
        /// Deletes a movie
        /// </summary>
        /// <param name="id"></param>
        [ProducesResponseType(200, Type = typeof(Movie))]
        [ProducesResponseType(404)]
        [Produces("application/json")]
        [HttpDelete]
        public JsonResult Delete(int id)
        {
            var result = _context.Movies.Find(id);

            if (result == null)
            {
                Response.StatusCode = 404;
                return new JsonResult(NotFound());
            }
            _context.Movies.Remove(result);
            _context.SaveChanges();

            Response.StatusCode = 200;
            return new JsonResult(Ok(result).Value  , options);
        }
    }
}
