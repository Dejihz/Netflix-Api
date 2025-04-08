using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace project6._1Api.Model
{
    [XmlRoot(ElementName = "Episode")]
    public class Episode
    {
        [XmlElement(ElementName = "episode_id")]
        public int episode_id { get; set; }

        [Required]
        [XmlElement(ElementName = "series_id")]
        public int series_id { get; set; }

        [Required]
        [Range(1, 50)]
        [XmlElement(ElementName = "season_number")]
        public int season_number { get; set; }

        [Required]
        [Range(1, 100)]
        [XmlElement(ElementName = "episode_number")]
        public int episode_number { get; set; }

        [Required]
        [StringLength(100)]
        [XmlElement(ElementName = "title")]
        public string title { get; set; }

        [Required]
        [Range(1, 120)]
        [XmlElement(ElementName = "duration")]
        public int duration { get; set; } // in minutes
    }
}
