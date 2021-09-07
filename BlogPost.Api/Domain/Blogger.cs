using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogPost.Api.Domain
{
    public class Blogger
    {
        [Key]
        public int BloggerId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ICollection<Blog> Blogs { get; set; }

    }
}
