using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace project6._1Api.Model
{
    [XmlRoot(ElementName = "Preferences")]
    public class Preferences
    {
        [XmlElement(ElementName = "preferences_id")]
        public int preferences_id { get; set; }

        [Required]
        [XmlElement(ElementName = "profile_id")]
        public int profile_id { get; set; }

        [Range(0, 21)]
        [XmlElement(ElementName = "min_age")]
        public int? min_age { get; set; }

        [StringLength(500)]
        [XmlElement(ElementName = "content_restrictions")]
        public string? content_restrictions { get; set; }
    }
}
