using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace project6._1Api.Entities
{
    public class LoginResponse
    {
        public string? Username { get; set; }
        public string? AccessToken { get; set; }
        public int ExpiresIn { get; set; }
    }
}
