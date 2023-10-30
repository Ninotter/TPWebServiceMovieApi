using Microsoft.AspNetCore.Mvc;
using TPWebServiceApiRestMovie.Context;
using TPWebServiceApiRestMovie.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace TPWebServiceApiRestMovie.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly ApiContext _context;
        private JsonSerializerOptions options = new JsonSerializerOptions();

        public PersonController(ApiContext context)
        {
            _context = context;
            options.MaxDepth = 0;
            options.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        }

        /// <summary>
        /// Creates a person
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///         "surname": "Buscemi",
        ///         "name": "Steve",
        ///         "birthDate": "1957-12-13T00:00:00.145Z"
        ///     }
        ///     
        /// </remarks>
        /// 
        [ProducesResponseType(201, Type = typeof(Person))]
        [ProducesResponseType(404)]
        [Produces("application/json")]
        [HttpPost]
        public JsonResult Create(Person person)
        {
            //Generates a new ID if the user mistakenly specified one
            person.Id = 0;
            //Clears specified movies
            person.MoviesPlayed.Clear();
            person.MoviesDirected.Clear();
            _context.Persons.Add(person);
            _context.SaveChanges();

            Response.StatusCode = 201;
           
            return new JsonResult(Ok(person).Value, options);
        }

        /// <summary>
        /// Gets a specific person
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

            if (result == null)
            {
                Response.StatusCode = 404;
                return new JsonResult(NotFound());
            }

            Response.StatusCode = 200;
            return new JsonResult(Ok(result).Value, options);
        }

        /// <summary>
        /// Edits a person
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///        "id" : 1,
        ///         "surname": "Buscemi",
        ///         "name": "Steve",
        ///         "birthDate": "1957-12-13T00:00:00.145Z"
        ///     }
        ///
        /// </remarks>
        [ProducesResponseType(200, Type = typeof(Person))]
        [ProducesResponseType(404)]
        [Produces("application/json")]
        [HttpPost]
        public JsonResult Edit(Person person)
        {
            var personInDb = _context.Persons.Find(person.Id);

            if (personInDb == null)
            {
                Response.StatusCode = 404;
                return new JsonResult(NotFound());
            }

            _context.Entry(personInDb).CurrentValues.SetValues(person);
            _context.SaveChanges();

            Response.StatusCode = 200;
            return new JsonResult(Ok(person).Value, options);
        }


        /// <summary>
        /// Deletes a specific person
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(200, Type = typeof(Person))]
        [ProducesResponseType(404)]
        public JsonResult Delete(int id)
        {
            var result = _context.Persons.Find(id);

            if (result == null)
            {
                Response.StatusCode = 404;
                return new JsonResult(NotFound());
            }
            _context.Persons.Remove(result);
            _context.SaveChanges();

            Response.StatusCode = 200;
            return new JsonResult(Ok(result).Value, options);
        }
    }
}
