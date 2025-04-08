using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace project6._1Api.Model
{
    [XmlRoot(ElementName = "Referral")]
    public class Referral
    {
        [Key]
        [XmlElement(ElementName = "id")]
        public int referral_id { get; set; }

        [Required]
        [XmlElement(ElementName = "referrerUserId")]
        public int referrer_user_id { get; set; }

        [Required]
        [XmlElement(ElementName = "referredUserId")]
        public int referred_user_id { get; set; }

        [XmlElement(ElementName = "discountApplied")]
        public bool discount_applied { get; set; } = false;
    }
}