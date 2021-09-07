using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogPost.Api.Services.DTO
{
    public class BlogDetail
    {
        public int BlogId { get; set; }

        public string BlogContent { get; set; }

        public string BloggerName { get; set; }

        public DateTime BlogCreatedOn { get; set; }

        public string BloggerEmail { get; set; }

        public string BloggerUserId { get; set; }

    }
}
