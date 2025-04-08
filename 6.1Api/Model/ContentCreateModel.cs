using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace project6._1Api.Model
{
    [XmlRoot(ElementName = "ContentCreateModel")]
    public class ContentCreateModel
    {
        [Required]
        [XmlElement(ElementName = "type")]
        public string type { get; set; } // "film" or "series"

        [Required]
        [StringLength(100)]
        [XmlElement(ElementName = "title")]
        public string title { get; set; }

        [Range(1888, 2100)]
        [XmlElement(ElementName = "release_year")]
        public int? release_year { get; set; }

        [StringLength(10)]
        [XmlElement(ElementName = "quality")]
        public string? quality { get; set; }

        [StringLength(50)]
        [XmlElement(ElementName = "classification")]
        public string? classification { get; set; }

        // Film-specific
        [Range(1, 600)]
        [XmlElement(ElementName = "duration")]
        public int? duration { get; set; }

        // Series-specific
        [Range(1, 50)]
        [XmlElement(ElementName = "number_of_seasons")]
        public int? number_of_seasons { get; set; }
    }
}
