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

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Level { get; set; }
        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}