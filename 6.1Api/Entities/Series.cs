using System.ComponentModel.DataAnnotations;

namespace project6._1Api.Entities
{
    public partial class Series
    {
        [Key]
        public int Series_id { get; set; }
        public int Content_id { get; set; }
        public int Number_of_seasons { get; set; }
    }
}