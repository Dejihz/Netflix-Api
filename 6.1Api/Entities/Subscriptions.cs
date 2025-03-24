using System.ComponentModel.DataAnnotations;

namespace project6._1Api.Entities
{
    public partial class Subscriptions
    {
        public Subscriptions() { }

        [Key]
        public int Subscription_id { get; set; } 

        public string Plan_type { get; set; } 

        public double Price { get; set; }

        public string Validity_period { get; set; }
    }
}