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
    public class BloggerRepository : IRepository<Blogger>
    {
        private readonly BloggingContext bloggingContext;
        private readonly ILogger<BloggerRepository> logger;

        public BloggerRepository(BloggingContext bloggingContext, ILogger<BloggerRepository> logger)
        {
            this.bloggingContext = bloggingContext;
            this.logger = logger;
        }
        public async Task<Blogger> Create(Blogger obj)
        {
            await bloggingContext.Bloggers.AddAsync(obj);
            await bloggingContext.SaveChangesAsync();
            return obj;
        }

        public async Task<bool> Delete(int id)
        {
            var blogger = await bloggingContext.Bloggers.FirstOrDefaultAsync(x => x.BloggerId.Equals(id));
            if (blogger != null)
            {
                bloggingContext.Bloggers.Remove(blogger);
                await bloggingContext.SaveChangesAsync();
                return true;
            }
            throw new EntityNotFoundException($"Blog user with id {id} could not be found.");
        }

        public async Task<IEnumerable<Blogger>> Get()
        {
            var bloggers = await bloggingContext.Bloggers.ToListAsync();
            return bloggers;
        }

        public async Task<IEnumerable<Blogger>> Get(Expression<Func<Blogger, bool>> predicate)
        {
            if (predicate == null)
            {
                return await Get();
            }

            var bloggers = await bloggingContext.Bloggers.Where(predicate).ToListAsync();
            return bloggers;
        }

        public async Task<Blogger> GetById(int id)
        {
            var blogger = await bloggingContext.Bloggers.FirstOrDefaultAsync(x => x.BloggerId == id);
            return blogger;
        }

        public async Task<Blogger> Update(Blogger obj)
        {
            var blogger = await bloggingContext.Bloggers.FirstOrDefaultAsync(x => x.BloggerId.Equals(obj.BloggerId));
            if (blogger != null)
            {
                blogger.Name = obj.Name;
                blogger.Email = obj.Email;

                await bloggingContext.SaveChangesAsync();
                return blogger;
            }
            throw new EntityNotFoundException($"Blogs user with id {obj.BloggerId} could not be found.");
        }
    }
}
