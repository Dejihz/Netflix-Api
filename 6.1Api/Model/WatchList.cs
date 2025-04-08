using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace project6._1Api.Model
{
    [XmlRoot(ElementName = "WatchList")]
    public class WatchList
    {
        [XmlElement(ElementName = "watchlist_id")]
        public int watchlist_id { get; set; }

        [Required]
        [XmlElement(ElementName = "profile_id")]
        public int profile_id { get; set; }

        [Required]
        [XmlElement(ElementName = "content_id")]
        public int content_id { get; set; }

        [Required]
        [XmlElement(ElementName = "added_date")]
        public DateTime added_date { get; set; }
    }
}
