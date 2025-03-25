using System.ComponentModel.DataAnnotations;

namespace project6._1Api.Model
{
    public class Subtitles
    {
        public int subtitle_id { get; set; }

        [Required]
        public int content_id { get; set; }

        [Required]
        [StringLength(20)]
        public string language { get; set; }
    }
}