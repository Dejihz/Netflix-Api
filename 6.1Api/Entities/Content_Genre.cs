using project6._1Api.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace project6._1Api.Entities
{
    public class Content_Genre
    {
        [ForeignKey("Content")]
        public int content_id { get; set; }

        [ForeignKey("Genre")]
        public int genre_id { get; set; }

        public virtual Contents Content { get; set; }
        public virtual Genres Genre { get; set; }
    }
}
