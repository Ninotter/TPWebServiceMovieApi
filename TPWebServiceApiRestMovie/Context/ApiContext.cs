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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Person>()
            .HasMany(p => p.MoviesPlayed)
            .WithMany(m => m.Actors)
            .UsingEntity("MovieActors");

            modelBuilder.Entity<Person>()
                .HasMany(p => p.MoviesDirected)
                .WithMany(m => m.Directors)
                .UsingEntity("MovieDirectors");
        }
    }
}
