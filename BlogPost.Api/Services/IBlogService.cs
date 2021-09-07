using BlogPost.Api.Services.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogPost.Api.Services
{
    public interface IBlogService
    {
        Task<List<BlogDetail>> GetBlogs();
        Task<bool> DeleteBlog(int blogId);
        Task<BlogDetail> GetBlogDetailsById(int id);
        Task<List<BlogDetail>> GetBlogDetailsByUserId(string userId);
        Task<BlogDetail> SaveBlogDetails(BlogDetail blogDetails);
    }
}