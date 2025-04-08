using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace project6._1Api.Model
{
    [XmlRoot(ElementName = "User")]
    public class User
    {
        [Key]
        [XmlElement(ElementName = "id")]
        public int user_id { get; set; }

        [Required]
        [XmlElement(ElementName = "email")]
        public string email { get; set; }

        [Required]
        [XmlElement(ElementName = "password")]
        public string password { get; set; }

        [Required]
        [XmlElement(ElementName = "accountStatus")]
        [AllowedValues("active", "inactive", "suspended")]
        public string account_status { get; set; }

        [XmlElement(ElementName = "subscriptionId")]
        public int? subscription_id { get; set; }

        [XmlElement(ElementName = "roleId")]
        public int? role_id { get; set; }

        [XmlElement(ElementName = "referredBy")]
        public int? referred_by { get; set; }

        /*[AllowNull]
        [XmlElement(ElementName = "refreshToken")]
        public string refreshToken { get; set; }

        [AllowNull]
        [XmlElement(ElementName = "refreshTokenExpiryTime")]
        public DateTime refreshTokenExpiryTime { get; set; }*/
    }
}
