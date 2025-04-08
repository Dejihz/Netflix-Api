using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace project6._1Api.Model
{
    [XmlRoot(ElementName = "WatchHistory")]
    public class WatchHistory
    {
        [XmlElement(ElementName = "history_id")]
        public int history_id { get; set; }

        [Required]
        [XmlElement(ElementName = "profile_id")]
        public int profile_id { get; set; }

        [Required]
        [XmlElement(ElementName = "content_id")]
        public int content_id { get; set; }

        [Required]
        [XmlElement(ElementName = "watch_date")]
        public DateTime watch_date { get; set; }

        [Required]
        [Range(1, 86400)]
        [XmlElement(ElementName = "watch_duration")]
        public int watch_duration { get; set; }

        [Required]
        [XmlElement(ElementName = "completed")]
        public bool completed { get; set; }
    }
}
