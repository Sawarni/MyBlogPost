using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogPost.Api.Domain
{
    public class BloggingContext:DbContext
    {
        public BloggingContext(DbContextOptions options):base(options)
        {

        }

        public DbSet<Blogger> Bloggers { get; set; }

        public DbSet<Blog> Blogs { get; set; }
    }
}
