using BlogPost.Api.Services;
using BlogPost.Api.Services.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlogPost.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly IBlogService service;
        private readonly ILogger<BlogsController> logger;

        public BlogsController(IBlogService service, ILogger<BlogsController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        // GET: api/<BlogsController>
        [HttpGet]
        public async Task<IEnumerable<BlogDetail>> Get()
        {
            return await service.GetBlogs();
        }

        // GET api/<BlogsController>/5
        [HttpGet("{id}")]
        public async Task<BlogDetail> Get(int id)
        {
            return await service.GetBlogDetailsById(id);
        }


        [HttpGet("{userId}")]
        public async Task<List<BlogDetail>> Get(string userId)
        {
            return await service.GetBlogDetailsByUserId(userId);
        }


        // POST api/<BlogsController>
        [HttpPost]
        public async Task<ActionResult<BlogDetail>> Post([FromBody] BlogDetail value)
        {
            var savedValue = await service.SaveBlogDetails(value);
            return CreatedAtAction(nameof(Get), savedValue.BlogId, savedValue);
        }

        // PUT api/<BlogsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<BlogDetail>> Put(int id, [FromBody] BlogDetail value)
        {
            if (id != value.BlogId)
                return new BadRequestResult();
            var savedValue = await service.SaveBlogDetails(value);
            return CreatedAtAction(nameof(Get), savedValue.BlogId, savedValue);
        }

        // DELETE api/<BlogsController>/5
        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            var deleted = await service.DeleteBlog(id);
            return deleted;
        }
    }

}
