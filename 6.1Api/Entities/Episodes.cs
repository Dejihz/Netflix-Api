using System.ComponentModel.DataAnnotations;

namespace project6._1Api.Entities
{
    public partial class Episodes
    {
        [Key]
        public int Episode_id { get; set; }
        public int Series_id { get; set; }
        public int Season_number { get; set; }
        public int Episode_number { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }
    }
}