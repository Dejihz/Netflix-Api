using System.ComponentModel.DataAnnotations.Schema;

namespace project6._1Api.Entities
{
    public class Profile_Genre
    {
        [ForeignKey("Profile")]
        public int Profile_id { get; set; }

        [ForeignKey("Genre")]
        public int Genre_id { get; set; }

        // Navigation properties
        public virtual Profiles Profile { get; set; }
        public virtual Genres Genre { get; set; }

    }
}
