using project6._1Api.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace project6._1Api.Entities
{
    public partial class ProfileGenrePreferences
    {
        public int Profile_id { get; set; }
        public int Genre_id { get; set; }

        [ForeignKey("profile_id")]
        public virtual Profiles Profile { get; set; }

        [ForeignKey("genre_id")]
        public virtual Genres Genre { get; set; }
    }
}