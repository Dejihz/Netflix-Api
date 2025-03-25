using System.ComponentModel.DataAnnotations;

namespace project6._1Api.Model
{
    public class ContentCreateModel
    {
        [Required]
        public string type { get; set; } // "film" or "series"

        [Required]
        [StringLength(100)]
        public string title { get; set; }

        [Range(1888, 2100)]
        public int? release_year { get; set; }

        [StringLength(10)]
        public string? quality { get; set; }

        [StringLength(50)]
        public string? classification { get; set; }

        // Film-specific
        [Range(1, 600)]
        public int? duration { get; set; }

        // Series-specific
        [Range(1, 50)]
        public int? number_of_seasons { get; set; }
    }
}
