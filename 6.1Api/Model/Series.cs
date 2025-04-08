using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace project6._1Api.Model
{
    [XmlRoot(ElementName = "Series")]
    public class Series
    {
        [XmlElement(ElementName = "series_id")]
        public int series_id { get; set; }

        [Required]
        [XmlElement(ElementName = "content_id")]
        public int content_id { get; set; }

        [Required]
        [Range(1, 50)]
        [XmlElement(ElementName = "number_of_seasons")]
        public int number_of_seasons { get; set; }
    }
}
