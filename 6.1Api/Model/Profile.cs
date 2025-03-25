using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace project6._1Api.Model
{
    public class Profile
    {
        public int profile_id { get; set; }

        [Required]
        public int user_id { get; set; }

        [Required]
        [StringLength(50)]
        public string name { get; set; }

        [StringLength(255)]
        public string? profile_photo { get; set; }

        [Range(0, 120)]
        public int? age { get; set; }

        [DefaultValue("English")]
        [StringLength(20)]
        public string language { get; set; } = "English";
    }
}