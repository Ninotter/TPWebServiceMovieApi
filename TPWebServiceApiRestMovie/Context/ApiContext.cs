using Microsoft.EntityFrameworkCore;
using TPWebServiceApiRestMovie.Models;

namespace TPWebServiceApiRestMovie.Context
{
    public class ApiContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Person> Persons { get; set; }
        public ApiContext(DbContextOptions<ApiContext> options) :base (options)
        {
            
        }
    }
}
