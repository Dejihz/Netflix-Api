using System.ComponentModel.DataAnnotations;

namespace project6._1Api.Entities
{
    public partial class Roles
    {
        public Roles()
        {
        }

        [Key]
        public int Role_id { get; set; }
        public string Role_name { get; set; }
        public string Permissions { get; set; }
    }
}