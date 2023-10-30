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
            options.ReferenceHandler = ReferenceHandler.IgnoreCycles;
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
                .FirstOrDefault();
            ;

            if (result == null || !result.IsDirector)
            {
                Response.StatusCode = 404;
                return new JsonResult(NotFound());
            }

            Response.StatusCode = 200;
            return new JsonResult(Ok(result).Value, options);
        }

        /// <summary>
        /// Gets all directors
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(Person))]
        [ProducesResponseType(404)]
        public JsonResult GetAll(int limit)
        {
            List<Person> result = _context.Persons
                .Include(p => p.MoviesPlayed)
                .Include(p => p.MoviesDirected)
                .Where(p => p.MoviesDirected.Count > 0)
                .Take(limit < 21 ? limit : 20)
                .ToList();

            Response.StatusCode = 200;
            return new JsonResult(Ok(result).Value, options);
        }

        /// <summary>
        /// Adds an director to a movie
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(200, Type = typeof(Person))]
        [ProducesResponseType(400)]
        [HttpPatch]
        public JsonResult AddToMovie(int idMovie, int idPerson)
        {
            var directorInDb = _context.Persons.Find(idPerson);

            var movieInDb = _context.Movies.Find(idMovie);

            if (directorInDb is null || movieInDb is null)
            {
                Response.StatusCode = 400;
                return new JsonResult(NotFound());
            }

            if (!directorInDb.MoviesDirected.Contains(movieInDb))
            {
                directorInDb.MoviesDirected.Add(movieInDb);
                _context.SaveChanges();
            }

            Response.StatusCode = 200;

            return new JsonResult(Ok(directorInDb).Value, options);
        }

        /// <summary>
        /// Remove an director from a movie
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(200, Type = typeof(Person))]
        [ProducesResponseType(400)]
        [HttpPatch]
        public JsonResult RemoveFromMovie(int idMovie, int idPerson)
        {
            var movieInDb = _context.Movies.Include(movie => movie.Directors).Where(movie => movie.Id == idMovie).FirstOrDefault();

            var directorInDb = _context.Persons.Include(director => director.MoviesDirected).Where(person => person.Id == idPerson).FirstOrDefault();

            if (directorInDb is null || movieInDb is null)
            {
                Response.StatusCode = 400;
                return new JsonResult(NotFound());
            }

            directorInDb.MoviesDirected.Remove(movieInDb);
            movieInDb.Directors.Remove(directorInDb);

            _context.SaveChanges();
            Response.StatusCode = 200;

            return new JsonResult(Ok(directorInDb).Value, options);
        }
    }
}
