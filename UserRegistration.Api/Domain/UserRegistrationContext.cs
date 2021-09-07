using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogPost.Api.Domain
{
    public class UserRegistrationContext:DbContext
    {
        public UserRegistrationContext(DbContextOptions options):base(options)
        {

        }

        public DbSet<AppUser> AppUsers { get; set; }
    }
}
