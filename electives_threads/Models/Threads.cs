using System;
using System.ComponentModel.DataAnnotations;

namespace electives_threads.Models
{
    public class Thread
    {
        public int ThreadID { get; set; }

        public int UserID { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; }

        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
