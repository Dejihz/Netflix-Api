using System.ComponentModel.DataAnnotations;

namespace project6._1Api.Model
{
    public class Content
    {
        public int content_id { get; set; }

        [Required]
        [StringLength(100)]
        public string title { get; set; }

        [Range(1888, 2100)]
        public int? release_year { get; set; }

        [StringLength(10)]
        public string? quality { get; set; }

        [StringLength(50)]
        public string? classification { get; set; }
    }
}