using BlogPost.Api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyBlogPost.Common.CustomException;
using MyBlogPost.Common.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BlogPost.Api.Repository
{
    public class BlogRepository : IRepository<Blog>
    {
        private readonly BloggingContext bloggingContext;
        private readonly ILogger<BlogRepository> logger;

        public BlogRepository(BloggingContext bloggingContext, ILogger<BlogRepository> logger)
        {
            this.bloggingContext = bloggingContext;
            this.logger = logger;
        }
        public async Task<Blog> Create(Blog blog)
        {
            await bloggingContext.Blogs.AddAsync(blog);
            await bloggingContext.SaveChangesAsync();
            return blog;
        }

        public async Task<bool> Delete(int id)
        {
            var blog = await bloggingContext.Blogs.FirstOrDefaultAsync(x => x.BlogId.Equals(id));
            if (blog != null)
            {
                bloggingContext.Blogs.Remove(blog);
                await bloggingContext.SaveChangesAsync();
                return true;
            }
            throw new EntityNotFoundException($"App user with id {id} could not be found.");

        }

        public async Task<IEnumerable<Blog>> Get()
        {
            var blogs = await bloggingContext.Blogs.Include(x => x.Blogger).ToListAsync();
            return blogs;
        }

        public async Task<IEnumerable<Blog>> Get(Expression<Func<Blog, bool>> predicate)
        {
            if (predicate == null)
            {
                return await Get();
            }

            var blogs = await bloggingContext.Blogs.Include(x => x.Blogger).Where(predicate).ToListAsync();
            return blogs;
        }

        public async Task<Blog> GetById(int id)
        {
            var user = await bloggingContext.Blogs.Include(x=>x.Blogger).FirstOrDefaultAsync(x => x.BlogId == id);
            return user;
        }

        public async Task<Blog> Update(Blog obj)
        {
            var blog = await bloggingContext.Blogs.FirstOrDefaultAsync(x => x.BlogId.Equals(obj.BlogId));
            if (blog != null)
            {
                blog.BlogContent = obj.BlogContent;

                await bloggingContext.SaveChangesAsync();
                return blog;
            }
            throw new EntityNotFoundException($"App user with id {obj.BlogId} could not be found.");
        }
    }
}
