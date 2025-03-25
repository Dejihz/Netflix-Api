using System.ComponentModel.DataAnnotations;

namespace project6._1Api.Entities
{
    public partial class Subtitles
    {
        [Key]
        public int Subtitle_id { get; set; }
        public int Content_id { get; set; }
        public string Language { get; set; }
    }
}