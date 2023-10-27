using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPWebServiceApiRestMovie.Context;
using TPWebServiceApiRestMovie.Models;

namespace TPWebServiceApiRestMovie.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly ApiContext _context;

        public MovieController(ApiContext context)
        {
            _context = context;
        }

        [HttpPost]
        public JsonResult Create(Movie movie)
        {
            _context.Movies.Add(movie);
            _context.SaveChanges();

            Response.StatusCode = 200;
            return new JsonResult(Ok(movie));
        }

        // Edit
        [HttpPost]
        public JsonResult Edit(Movie movie)
        {
            var movieInDb = _context.Movies.Find(movie.Id);

            if(movieInDb == null) {
                Response.StatusCode = 404;
                return new JsonResult(NotFound());
            }

            _context.SaveChanges();

            Response.StatusCode = 200;
            return new JsonResult(Ok(movie));
        }

        // Get
        [HttpGet]
        public JsonResult Get(int id)
        {
            var result = _context.Movies.Find(id);

            if(result == null)
            {
                Response.StatusCode = 404;
                return new JsonResult(NotFound());
            }
            Response.StatusCode = 200;
            return new JsonResult(Ok(result));
        }

        //Delete
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
            return new JsonResult(Ok(result));
        }
    }
}
