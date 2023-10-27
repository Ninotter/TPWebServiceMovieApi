using System.ComponentModel.DataAnnotations;

namespace TPWebServiceApiRestMovie.Models
{
    public class Person
    {
        [Key]
        public int Id { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }

        public bool IsActor { get => MoviesPlayed.Any(); }
        public bool IsDirector { get => MoviesDirected.Any(); }

        public List<Movie> MoviesPlayed { get; set; } = new List<Movie>();
        public List<Movie> MoviesDirected { get; set; } = new List<Movie>();
    }
}
