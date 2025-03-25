using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace project6._1Api.Model
{
    [XmlRoot(ElementName = "Role")]
    public class Role
    {
        [Key]
        [XmlElement(ElementName = "id")]
        public int role_id { get; set; }

        [Required]
        [XmlElement(ElementName = "roleName")]
        [StringLength(20)]
        public string role_name { get; set; }

        [Required]
        [XmlElement(ElementName = "permissions")]
        public string permissions { get; set; }
    }
}