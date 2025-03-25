using System.ComponentModel.DataAnnotations;

namespace project6._1Api.Entities
{
    public partial class Profiles
    {
        [Key]
        public int Profile_id { get; set; }
        public int User_id { get; set; }
        public string Name { get; set; }
        public string? Profile_photo { get; set; }
        public int? Age { get; set; }
        public string Language { get; set; } = "English";
    }
}