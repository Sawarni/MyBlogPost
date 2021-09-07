using AutoMapper;
using BlogPost.Api.Domain;
using BlogPost.Api.Services.DTO;
using Microsoft.Extensions.Logging;
using MyBlogPost.Common.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogPost.Api.Services
{
    public class BloggerService : IBloggerService
    {
        private readonly IRepository<Blogger> bloggerRepository;
        private readonly ILogger<BlogService> logger;
        private readonly IMapper mapper;

        public BloggerService(IRepository<Blogger> bloggerRepository, ILogger<BlogService> logger, IMapper mapper)
        {
            this.bloggerRepository = bloggerRepository;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task<BlogUser> SaveBlogDetails(BlogUser bloggerDetails)
        {
            var blogger = mapper.Map<Blogger>(bloggerDetails);
            var bloggerinDb = await bloggerRepository.Get(x => x.UserId == bloggerDetails.UserId);
            if (bloggerinDb != null)
            {
                blogger = await bloggerRepository.Update(blogger);
            }
            else
            {
                blogger = await bloggerRepository.Create(blogger);
            }

            var savedBlogDetails = mapper.Map<BlogUser>(blogger);
            return savedBlogDetails;

        }

        public async Task<bool> DeleteBlog(string userId)
        {
            var bloggerinDb = await bloggerRepository.Get(x => x.UserId == userId);
            int? bloggerId = bloggerinDb.FirstOrDefault()?.BloggerId;
            if (bloggerId.HasValue)
            {
                var deleted = await bloggerRepository.Delete(bloggerId.Value);
                return deleted;
            }
            return false;
        }



    }
}
