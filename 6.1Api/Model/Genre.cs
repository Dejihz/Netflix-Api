using System.ComponentModel.DataAnnotations;

namespace project6._1Api.Model
{
    public class Genre
    {
        public int genre_id { get; set; }

        [Required]
        [StringLength(50)]
        public string genre_name { get; set; }


        public class ContentGenreAssignmentModel
        {
            [Required]
            public int content_id { get; set; }

            [Required]
            public int genre_id { get; set; }
        }

        public class BulkGenreAssignmentModel
        {
            [Required]
            public int content_id { get; set; }

            [Required]
            public List<int> genre_ids { get; set; }
        }
    }
}