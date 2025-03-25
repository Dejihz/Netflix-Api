using System.ComponentModel.DataAnnotations;

namespace project6._1Api.Model
{
    public class Series
    {
        public int series_id { get; set; }

        [Required]
        public int content_id { get; set; }

        [Required]
        [Range(1, 50)]
        public int number_of_seasons { get; set; }
    }
}