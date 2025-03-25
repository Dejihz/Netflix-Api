using System.ComponentModel.DataAnnotations;

namespace project6._1Api.Model
{
    public class WatchList
    {
        public int watchlist_id { get; set; }

        [Required]
        public int profile_id { get; set; }

        [Required]
        public int content_id { get; set; }

        [Required]
        public DateTime added_date { get; set; }
    }
}