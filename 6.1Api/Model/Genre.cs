using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace project6._1Api.Model
{
    [XmlRoot(ElementName = "Genre")]
    public class Genre
    {
        [XmlElement(ElementName = "genre_id")]
        public int genre_id { get; set; }

        [Required]
        [StringLength(50)]
        [XmlElement(ElementName = "genre_name")]
        public string genre_name { get; set; }

        [XmlRoot(ElementName = "ContentGenreAssignment")]
        public class ContentGenreAssignmentModel
        {
            [Required]
            [XmlElement(ElementName = "content_id")]
            public int content_id { get; set; }

            [Required]
            [XmlElement(ElementName = "genre_id")]
            public int genre_id { get; set; }
        }

        [XmlRoot(ElementName = "BulkGenreAssignment")]
        public class BulkGenreAssignmentModel
        {
            [Required]
            [XmlElement(ElementName = "content_id")]
            public int content_id { get; set; }

            [Required]
            [XmlArray(ElementName = "genre_ids")]
            [XmlArrayItem(ElementName = "genre_id")]
            public List<int> genre_ids { get; set; }
        }
    }
}
