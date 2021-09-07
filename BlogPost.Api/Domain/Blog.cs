using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogPost.Api.Domain
{
    public class Blog
    {
        [Key]
        public int BlogId { get; set; }

        [Required, MinLength(5, ErrorMessage = "At least five characters are required.")]
        public string BlogContent { get; set; }

        public DateTime CreatedOn { get; set; }
        public Blogger Blogger { get; set; }
    }
}
