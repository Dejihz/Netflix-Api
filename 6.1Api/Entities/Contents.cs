using System.ComponentModel.DataAnnotations;

namespace project6._1Api.Entities
{
    public partial class Contents
    {
        [Key]
        public int Content_id { get; set; }
        public string Title { get; set; }
        public int? Release_year { get; set; }
        public string? Quality { get; set; }
        public string? Classification { get; set; }

        public virtual ICollection<Content_Genre> ContentGenres { get; set; } = new List<Content_Genre>();
    }


}