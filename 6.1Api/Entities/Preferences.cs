using System.ComponentModel.DataAnnotations;

namespace project6._1Api.Entities
{
    public partial class Preferences
    {
        [Key]
        public int Preferences_id { get; set; }
        public int Profile_id { get; set; }
        public int? Min_age { get; set; }
        public string? Content_restrictions { get; set; }
    }
}