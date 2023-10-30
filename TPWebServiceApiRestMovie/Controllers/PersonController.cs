using Microsoft.AspNetCore.Mvc;
using TPWebServiceApiRestMovie.Context;
using TPWebServiceApiRestMovie.Models;

namespace TPWebServiceApiRestMovie.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly ApiContext _context;

        public PersonController(ApiContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a person
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     "surname": "Buscemi",
        ///     "name": "Steve",
        ///     "birthDate": "1957-12-13T00:00:00.145Z"
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
            //person.MoviesPlayed.Clear();
            //person.MoviesDirected.Clear();
            _context.Persons.Add(person);
            _context.SaveChanges();

            Response.StatusCode = 201;
            return new JsonResult(Ok(person));
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
            var result = _context.Persons.Find(id);

            if (result == null)
            {
                return new JsonResult(NotFound());
            }

            return new JsonResult(Ok(result));
        }
    }
}
