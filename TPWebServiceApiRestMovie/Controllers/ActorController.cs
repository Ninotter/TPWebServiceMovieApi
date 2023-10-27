using Microsoft.AspNetCore.Mvc;
using TPWebServiceApiRestMovie.Context;

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

        // Get
        [HttpGet]
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