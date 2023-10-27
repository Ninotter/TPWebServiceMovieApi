using System.ComponentModel.DataAnnotations;

namespace TPWebServiceApiRestMovie.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(128)]
        public string Title { get; set; }
        [MaxLength(2048)]
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
