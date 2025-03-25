using System.ComponentModel.DataAnnotations;

namespace project6._1Api.Model
{
    public class ProfileGenrePreference
    {
        [Required]
        public int profile_id { get; set; }

        [Required]
        public int genre_id { get; set; }
    }
}