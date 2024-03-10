using System;
using System.ComponentModel.DataAnnotations;

namespace electives_threads.Models
{
    public class Comment
    {
        public int CommentID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int ThreadID { get; set; }

        [Required]
        public string Content { get; set; }

        public int Likes { get; set; }

        public int Dislikes { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

  
    }
}
