using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;

namespace project6._1Api.Entities
{
    public partial class Users
    {
        public Users()
        {

        }
        [Key]
        public int User_id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Account_status { get; set; }
        public int? Subscription_id { get; set; }
        public int? Role_id { get; set; }
        public int? Referred_by { get; set; }
        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}