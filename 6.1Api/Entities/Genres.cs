using System.ComponentModel.DataAnnotations;

namespace project6._1Api.Entities
{
    public partial class Genres
    {
        [Key]
        public int Genre_id { get; set; }
        public string Genre_name { get; set; }
    }
}