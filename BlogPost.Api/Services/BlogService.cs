using BlogPost.Api.Domain;
using BlogPost.Api.Services.DTO;
using Microsoft.Extensions.Logging;
using MyBlogPost.Common.RepositoryBase;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogPost.Api.Services
{
    public class BlogService : IBlogService
    {
        private readonly IRepository<Blog> blogRepository;
        private readonly ILogger<BlogService> logger;
        private readonly IMapper mapper;

        public BlogService(IRepository<Blog> blogRepository, ILogger<BlogService> logger, IMapper mapper)
        {
            this.blogRepository = blogRepository;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task<List<BlogDetail>> GetBlogs()
        {
            var blog = await blogRepository.Get();
            var blogDetails = mapper.Map<List<BlogDetail>>(blog);
            return blogDetails;
        }
        public async Task<BlogDetail> GetBlogDetailsById(int id)
        {
            var blog = await blogRepository.GetById(id);
            var blogDetails = mapper.Map<BlogDetail>(blog);
            return blogDetails;
        }


        public async Task<List<BlogDetail>> GetBlogDetailsByUserId(string userId)
        {
            var blog = await blogRepository.Get(x => x.Blogger.UserId == userId);
            var blogDetails = mapper.Map<List<BlogDetail>>(blog);
            return blogDetails;
        }

        public async Task<BlogDetail> SaveBlogDetails(BlogDetail blogDetails)
        {
            var blog = mapper.Map<Blog>(blogDetails);
            if (blog.BlogId > 0)
            {
                blog = await blogRepository.Update(blog);
            }
            else
            {
                blog = await blogRepository.Create(blog);
            }

            var savedBlogDetails = mapper.Map<BlogDetail>(blog);
            return savedBlogDetails;

        }

        public async Task<bool> DeleteBlog(int blogId)
        {
            var deleted = await blogRepository.Delete(blogId);
            return deleted;
        }
    }
}
