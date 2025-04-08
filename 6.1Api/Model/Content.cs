using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace project6._1Api.Model
{
    [XmlRoot(ElementName = "Content")]
    public class Content
    {
        [Key]
        [XmlElement(ElementName = "content_id")]
        public int content_id { get; set; }

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
    }
}
