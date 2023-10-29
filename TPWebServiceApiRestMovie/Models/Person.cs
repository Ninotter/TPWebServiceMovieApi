using System.ComponentModel.DataAnnotations;

namespace TPWebServiceApiRestMovie.Models
{
    public class Person
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }

        public bool IsActor { get => MoviesPlayed.Any(); }
        public bool IsDirector { get => MoviesDirected.Any(); }

        public List<Movie> MoviesPlayed { get; set; } = new List<Movie>();
        public List<Movie> MoviesDirected { get; set; } = new List<Movie>();
    }
}
