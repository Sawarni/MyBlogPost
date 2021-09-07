using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyBlogPost.Common.CustomException;
using MyBlogPost.Common.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogPost.Api.Domain;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlogPost.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUsersController : ControllerBase
    {
        private readonly IRepository<AppUser> appUserRepository;
        private readonly ILogger<AppUsersController> logger;

        public AppUsersController(IRepository<AppUser> appUserRepository, ILogger<AppUsersController> logger)
        {
            this.appUserRepository = appUserRepository;
            this.logger = logger;
        }
        // GET: api/<AppUsersController>
        [HttpGet]
        public async Task<IEnumerable<AppUser>> Get()
        {
            return await appUserRepository.Get();
        }

        // GET api/<AppUsersController>/5
        [HttpGet("{id}")]
        public async Task<AppUser> Get(int id)
        {
            return await appUserRepository.GetById(id);
        }

        // POST api/<AppUsersController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AppUser value)
        {
            if (value == null)
            {
                return BadRequest();
            }

            var user = await appUserRepository.Create(value);
            return Ok(user);

        }

        // PUT api/<AppUsersController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] AppUser value)
        {
            if (id != value.AppUserId)
            {
                return BadRequest("The id is not same as as the object.");
            }

            try
            {
                var user = await appUserRepository.Update(value);
                return Ok(user);

            }
            catch (EntityNotFoundException)
            {

                return NotFound($"The id {id} is not found.");
            }
        }

        // DELETE api/<AppUsersController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var deleted = await appUserRepository.Delete(id);
                if (deleted)
                    return Ok("User deleted successfully.");
                else
                    return Ok("User deletion failed.");

            }
            catch (EntityNotFoundException)
            {

                return NotFound($"The id {id} is not found.");
            }
        }
    }
}
