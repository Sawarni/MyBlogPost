using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyBlogPost.Common.CustomException;
using MyBlogPost.Common.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BlogPost.Api.Domain;

namespace BlogPost.Api.Repository
{
    public class AppUserRepository : IRepository<AppUser>
    {
        private readonly UserRegistrationContext userRegistrationContext;
        private readonly ILogger<AppUserRepository> logger;

        public AppUserRepository(UserRegistrationContext userRegistrationContext, ILogger<AppUserRepository> logger)
        {
            this.userRegistrationContext = userRegistrationContext;
            this.logger = logger;
        }
        public async Task<AppUser> Create(AppUser appUser)
        {
            await userRegistrationContext.AppUsers.AddAsync(appUser);
            await userRegistrationContext.SaveChangesAsync();
            return appUser;
        }

        public async Task<bool> Delete(int id)
        {
            var user = await userRegistrationContext.AppUsers.FirstOrDefaultAsync(x => x.AppUserId.Equals(id));
            if (user != null)
            {
                userRegistrationContext.AppUsers.Remove(user);
                await userRegistrationContext.SaveChangesAsync();
                return true;
            }
            throw new EntityNotFoundException($"App user with id {id} could not be found.");
        }

        public async Task<IEnumerable<AppUser>> Get()
        {
            var users = await userRegistrationContext.AppUsers.ToListAsync();
            return users;
        }

        public async Task<IEnumerable<AppUser>> Get(Expression<Func<AppUser, bool>> predicate)
        {
            if (predicate == null)
            {
                return await Get();
            }

            var users = await userRegistrationContext.AppUsers.Where(predicate).ToListAsync();
            return users;
        }

        public async Task<AppUser> GetById(int id)
        {
            var user = await userRegistrationContext.AppUsers.FirstOrDefaultAsync(x => x.AppUserId == id);
            return user;
        }

        public async Task<AppUser> Update(AppUser obj)
        {

            var user = await userRegistrationContext.AppUsers.FirstOrDefaultAsync(x => x.AppUserId.Equals(obj.AppUserId));
            if (user != null)
            {
                user.EmailId = obj.EmailId;
                user.FirstName = obj.FirstName;
                user.LastName = obj.LastName;
                user.UserId = obj.UserId;

                await userRegistrationContext.SaveChangesAsync();
                return user;
            }
            throw new EntityNotFoundException($"App user with id {obj.AppUserId} could not be found.");
        }
    }
}
