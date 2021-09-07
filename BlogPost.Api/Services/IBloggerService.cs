using BlogPost.Api.Services.DTO;
using System.Threading.Tasks;

namespace BlogPost.Api.Services
{
    public interface IBloggerService
    {
        Task<bool> DeleteBlog(string userId);
        Task<BlogUser> SaveBlogDetails(BlogUser bloggerDetails);
    }
}