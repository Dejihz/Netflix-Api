using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace project6._1Api.Model
{
    [XmlRoot(ElementName = "Subscription")]
    public class Subscription
    {
        [Key]
        [XmlElement(ElementName = "subscription_id")]
        public int Subscription_id { get; set; }

        [Required]
        [XmlElement(ElementName = "plan_type")]
        [MaxLength(10)]
        [AllowedValues("trial", "standard","family")]
        public string Plan_type { get; set; }

        [Required]
        [XmlElement(ElementName = "price")]
        public double Price { get; set; }

        [Required]
        [XmlElement(ElementName = "validity_period")]
        [MaxLength(20)]
        [AllowedValues("1 month", "1 year", "lifetime")]
        public string Validity_period { get; set; }
    }
}