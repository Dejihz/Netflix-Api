using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace project6._1Api.Model
{
    [XmlRoot(ElementName = "Subtitles")]
    public class Subtitles
    {
        [XmlElement(ElementName = "subtitle_id")]
        public int subtitle_id { get; set; }

        [Required]
        [XmlElement(ElementName = "content_id")]
        public int content_id { get; set; }

        [Required]
        [StringLength(20)]
        [XmlElement(ElementName = "language")]
        public string language { get; set; }
    }
}
