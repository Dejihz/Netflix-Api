using System.ComponentModel.DataAnnotations;
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
    }
}