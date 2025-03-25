using System.ComponentModel.DataAnnotations;

namespace project6._1Api.Entities
{
    public partial class Referrals
    {
        public Referrals()
        {
        }

        [Key]
        public int Referral_id { get; set; }
        public int Referrer_user_id { get; set; }
        public int Referred_user_id { get; set; }
        public bool Discount_applied { get; set; }
    }
}