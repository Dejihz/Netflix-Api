using System.ComponentModel.DataAnnotations;
using project6._1Api.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace project6._1Api.Entities
{
    public class Genres
    {
        [Key]
        public int genre_id { get; set; }

        public string genre_name { get; set; }

        public virtual ICollection<Content_Genre> ContentGenres { get; set; } = new List<Content_Genre>();


        public virtual ICollection<Profile_Genre> GenrePreferences { get; set; } = new List<Profile_Genre>();

    }
}