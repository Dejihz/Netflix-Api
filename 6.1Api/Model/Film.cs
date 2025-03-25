using System.ComponentModel.DataAnnotations;

namespace project6._1Api.Model
{
    public class Film
    {
        public int film_id { get; set; }

        [Required]
        public int content_id { get; set; }

        [Required]
        [Range(1, 600)] 
        public int duration { get; set; } 
    }
}