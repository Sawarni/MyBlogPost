using AutoMapper;
using BlogPost.Api.Domain;
using BlogPost.Api.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogPost.Api.Services.Mapper
{
    public class BlogProfile:Profile
    {
        public BlogProfile()
        {
            //CreateMap<BlogDetail, Blog>()
            //    .ForMember(x => x.Blogger.UserId, cfg => cfg.MapFrom(y => y.BloggerUserId))
            //    .ForMember(x => x.Blogger.Name, cfg => cfg.MapFrom(y => y.BloggerName))
            //    .ForMember(x => x.Blogger.Email, cfg => cfg.MapFrom(y => y.BloggerEmail));

            CreateMap<Blog, BlogDetail>()
                .ForMember(x => x.BloggerUserId, cfg => cfg.MapFrom(y => y.Blogger.UserId))
                .ForMember(x => x.BloggerName, cfg => cfg.MapFrom(y => y.Blogger.Name))
                .ForMember(x => x.BloggerEmail, cfg => cfg.MapFrom(y => y.Blogger.Email));
        }
        
    }
}
