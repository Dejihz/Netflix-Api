using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace project6._1Api.Model
{
    [XmlRoot(ElementName = "User")]
    public class User
    {
        [Key]
        [XmlElement(ElementName = "id")]
        public int id { get; set; }

        [Required]
        [XmlElement(ElementName = "username")]
        public string username { get; set; }

        [Required]
        [XmlElement(ElementName = "password")]
        public string password { get; set; }

        [Required]
        [XmlElement(ElementName = "level")]
        [AllowedValues("superuser", "admin", "guest")]
        public string level { get; set; }
    }
}
