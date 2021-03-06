using AutoMapper;
using BlogPost.Api.Domain;
using BlogPost.Api.Repository;
using BlogPost.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MyBlogPost.Common.InterserviceContracts;
using MyBlogPost.Common.MQ;
using MyBlogPost.Common.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogPost.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("DbConnection");
            services.AddDbContext<BloggingContext>(option => option.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)).EnableSensitiveDataLogging());
            services.AddAutoMapper(typeof(Startup));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BlogPost.Api", Version = "v1" });
            });

            services.AddTransient<IRepository<Blogger>, BloggerRepository>();

            services.AddTransient<IRepository<Blog>, BlogRepository>();

            services.AddTransient<IBlogService, BlogService>();

            services.AddTransient<IBloggerService, BloggerService>();
            EnableMessageReceive(services);




        }

        private void EnableMessageReceive(IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var serviceClientSettingsConfig = Configuration.GetSection("RabbitMq");
            var serviceClientSettings = serviceClientSettingsConfig.Get<RabbitMqConfiguration>();

            Subscriber subscriber = new Subscriber(serviceClientSettings.Hostname, serviceClientSettings.UserName, serviceClientSettings.Password);
            
            subscriber.ListenToQueue(async (e, payload) =>
             {

                UserDataExchange exchange = new UserDataExchange().GetFromString(payload);
                Blogger blogger = new Blogger
                {
                    Email = exchange.EmailId,
                    Name = exchange.Name,
                    UserId = exchange.UserId
                };

                IRepository<Blogger> bloggerRepository = provider.GetRequiredService<IRepository<Blogger>>();

                switch (e)
                {

                    case ExchaneEvents.UserAdd:
                       await bloggerRepository.Create(blogger);
                        break;

                    case ExchaneEvents.UserUpdate:
                       await bloggerRepository.Update(blogger);
                        break;
                    case ExchaneEvents.UserDelete:
                        var bloggerDb = await bloggerRepository.Get(x => x.UserId.Equals(blogger.UserId));
                        await bloggerRepository.Delete(bloggerDb.First().BloggerId);
                        break;


                    default:
                        break;
                }



            }, "user.blog");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BlogPost.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            
        }
    }
}
