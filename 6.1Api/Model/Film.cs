using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace project6._1Api.Model
{
    [XmlRoot(ElementName = "Film")]
    public class Film
    {
        [XmlElement(ElementName = "film_id")]
        public int film_id { get; set; }

        [Required]
        [XmlElement(ElementName = "content_id")]
        public int content_id { get; set; }

        [Required]
        [Range(1, 600)]
        [XmlElement(ElementName = "duration")]
        public int duration { get; set; }
    }
}
