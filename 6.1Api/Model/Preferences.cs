using System.ComponentModel.DataAnnotations;

namespace project6._1Api.Model
{
    public class Preferences
    {
        public int preferences_id { get; set; }

        [Required]
        public int profile_id { get; set; }

        [Range(0, 21)]
        public int? min_age { get; set; }

        [StringLength(500)]
        public string? content_restrictions { get; set; }
    }
}