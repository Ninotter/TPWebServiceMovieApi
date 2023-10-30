using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TPWebServiceApiRestMovie.Models
{
    public class Movie
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(128)]
        public string Title { get; set; }

        [Required]
        [MaxLength(2048)]
        public string Description { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        public List<Person> Actors { get; set; } = new List<Person>();
        public List<Person> Directors { get; set; } = new List<Person>();
    }
}
