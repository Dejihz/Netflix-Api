using System.ComponentModel.DataAnnotations;

namespace project6._1Api.Model
{
    public class WatchHistory
    {
        public int history_id { get; set; }

        [Required]
        public int profile_id { get; set; }

        [Required]
        public int content_id { get; set; }

        [Required]
        public DateTime watch_date { get; set; }

        [Required]
        [Range(1, 86400)] // Max 24 hours in seconds
        public int watch_duration { get; set; }

        [Required]
        public bool completed { get; set; }
    }
}