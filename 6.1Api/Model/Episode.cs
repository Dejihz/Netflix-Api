using System.ComponentModel.DataAnnotations;

namespace project6._1Api.Model
{
    public class Episode
    {
        public int episode_id { get; set; }

        [Required]
        public int series_id { get; set; }

        [Required]
        [Range(1, 50)]
        public int season_number { get; set; }

        [Required]
        [Range(1, 100)]
        public int episode_number { get; set; }

        [Required]
        [StringLength(100)]
        public string title { get; set; }

        [Required]
        [Range(1, 120)]
        public int duration { get; set; } // in minutes
    }
}