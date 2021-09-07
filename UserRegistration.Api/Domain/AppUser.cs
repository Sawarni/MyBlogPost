using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogPost.Api.Domain
{
    public class AppUser
    {
        [Key]
        public int AppUserId { get; set; }

        [Required, MinLength(5)]
        public string UserId { get; set; }

        [Required, MinLength(1)]
        public string FirstName { get; set; }

        [Required, MinLength(1)]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string EmailId { get; set; }


    }
}
