using AutoMapper;
using BlogPost.Api.Domain;
using BlogPost.Api.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogPost.Api.Services.Mapper
{
    public class BloggerProfile : Profile
    {
        public BloggerProfile()
        {
            CreateMap<BlogUser, Blogger>()
                .ForMember(x => x.Name, cfg => cfg.MapFrom(y => $"{y.FirstName} {y.LastName}".Trim()));

        }
    }
}
