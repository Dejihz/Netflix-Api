using System.ComponentModel.DataAnnotations;

namespace project6._1Api.Entities
{
    public partial class WatchHistories
    {
        [Key]
        public int History_id { get; set; }
        public int Profile_id { get; set; }
        public int Content_id { get; set; }
        public DateTime Watch_date { get; set; }
        public int Watch_duration { get; set; }
        public bool Completed { get; set; }
    }
}