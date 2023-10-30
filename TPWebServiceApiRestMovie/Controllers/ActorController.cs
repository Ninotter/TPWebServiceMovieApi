using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TPWebServiceApiRestMovie.Context;
using TPWebServiceApiRestMovie.Models;

namespace TPWebServiceApiRestMovie.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ActorController : ControllerBase
    {
        private readonly ApiContext _context;

        public ActorController(ApiContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets a specific actor
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(Person))]
        [ProducesResponseType(400)]
        public JsonResult Get(int id)
        {
            var result = _context.Persons.Find(id);

            if (result == null || !result.IsActor)
            {
                return new JsonResult(NotFound());
            }

            return new JsonResult(Ok(result));
        }
    }
}