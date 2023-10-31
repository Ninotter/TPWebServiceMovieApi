using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using TPWebServiceApiRestMovie.Context;
using TPWebServiceApiRestMovie.Models;

namespace TPWebServiceApiRestMovie.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ActorController : ControllerBase
    {
        private readonly ApiContext _context;
        private JsonSerializerOptions options = new JsonSerializerOptions();

        public ActorController(ApiContext context)
        {
            _context = context;
            options.MaxDepth = 0;
            options.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        }

        /// <summary>
        /// Gets a specific actor
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

            if (result == null || !result.IsActor)
            {
                Response.StatusCode = 404;
                return new JsonResult(NotFound());
            }

            Response.StatusCode = 200;
            return new JsonResult(Ok(result).Value, options);
        }

        /// <summary>
        /// Gets all actors
        /// </summary>
        /// <param name="limit">20 or lower.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(Person))]
        [ProducesResponseType(404)]
        public JsonResult GetAll(int limit = 20)
        {
            List<Person> result = _context.Persons
                .Include(p => p.MoviesPlayed)
                .Include(p => p.MoviesDirected)
                .Where(p => p.MoviesPlayed.Count > 0)
                .Take(limit < 21 ? limit : 20)
                .ToList();

            Response.StatusCode = 200;
            return new JsonResult(Ok(result).Value, options);
        }

        /// <summary>
        /// Adds an actor to a movie
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(200, Type = typeof(Person))]
        [ProducesResponseType(400)]
        [HttpPatch]
        public JsonResult AddToMovie(int idMovie, int idPerson)
        {
            var actorInDb = _context.Persons.Find(idPerson);

            var movieInDb = _context.Movies.Find(idMovie);

            if (actorInDb is null || movieInDb is null)
            {
                Response.StatusCode = 400;
                return new JsonResult(NotFound());
            }

            if (!actorInDb.MoviesPlayed.Contains(movieInDb))
            {
                actorInDb.MoviesPlayed.Add(movieInDb);
                _context.SaveChanges();
            }

            Response.StatusCode = 200;

            return new JsonResult(Ok(actorInDb).Value, options);
        }


        /// <summary>
        /// Remove an actor from a movie
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(200, Type = typeof(Person))]
        [ProducesResponseType(400)]
        [HttpPatch]
        public JsonResult RemoveFromMovie(int idMovie, int idPerson)
        {
            var movieInDb = _context.Movies.Include(movie => movie.Actors).Where(movie => movie.Id == idMovie).FirstOrDefault() ;

            var actorInDb = _context.Persons.Include(actor => actor.MoviesPlayed).Where(person => person.Id == idPerson).FirstOrDefault();

            if (actorInDb is null || movieInDb is null)
            {
                Response.StatusCode = 400;
                return new JsonResult(NotFound());
            }

            actorInDb.MoviesPlayed.Remove(movieInDb);
            movieInDb.Actors.Remove(actorInDb);

            _context.SaveChanges();
            Response.StatusCode = 200;

            return new JsonResult(Ok(actorInDb).Value, options);
        }
    }
}