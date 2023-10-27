using Microsoft.AspNetCore.Mvc;
using TPWebServiceApiRestMovie.Context;

namespace TPWebServiceApiRestMovie.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DirectorController : ControllerBase
    {
        private readonly ApiContext _context;

        public DirectorController(ApiContext context)
        {
            _context = context;
        }

        // Get
        [HttpGet]
        public JsonResult Get(int id)
        {
            var result = _context.Persons.Find(id);

            if (result == null || !result.IsDirector)
            {
                return new JsonResult(NotFound());
            }

            return new JsonResult(Ok(result));
        }
    }
}
