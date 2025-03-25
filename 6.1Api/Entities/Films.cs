using System.ComponentModel.DataAnnotations;

namespace project6._1Api.Entities
{
    public partial class Films
    {
        [Key]
        public int Film_id { get; set; }
        public int Content_id { get; set; }
        public int Duration { get; set; }
    }
}