using System.ComponentModel.DataAnnotations;

namespace project6._1Api.Entities
{
    public partial class WatchLists
    {
        [Key]
        public int Watchlist_id { get; set; }
        public int Profile_id { get; set; }
        public int Content_id { get; set; }
        public DateTime Added_date { get; set; }
    }
}