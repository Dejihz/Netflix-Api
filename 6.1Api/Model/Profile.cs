using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace project6._1Api.Model
{
    [XmlRoot(ElementName = "Profile")]
    public class Profile
    {
        [XmlElement(ElementName = "profile_id")]
        public int profile_id { get; set; }

        [Required]
        [XmlElement(ElementName = "user_id")]
        public int user_id { get; set; }

        [Required]
        [StringLength(50)]
        [XmlElement(ElementName = "name")]
        public string name { get; set; }

        [StringLength(255)]
        [XmlElement(ElementName = "profile_photo")]
        public string? profile_photo { get; set; }

        [Range(0, 120)]
        [XmlElement(ElementName = "age")]
        public int? age { get; set; }

        [DefaultValue("English")]
        [StringLength(20)]
        [XmlElement(ElementName = "language")]
        public string language { get; set; } = "English";
    }
}
