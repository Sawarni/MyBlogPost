using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyBlogPost.Common.CustomException;
using MyBlogPost.Common.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogPost.Api.Domain;
using Microsoft.Extensions.Options;
using MyBlogPost.Common.MQ;
using MyBlogPost.Common.InterserviceContracts;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlogPost.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUsersController : ControllerBase
    {
        private readonly IRepository<AppUser> appUserRepository;
        private readonly ILogger<AppUsersController> logger;
        private readonly IOptions<RabbitMqConfiguration> rabbitMqOptions;

        public AppUsersController(IRepository<AppUser> appUserRepository, ILogger<AppUsersController> logger, IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {
            this.appUserRepository = appUserRepository;
            this.logger = logger;
            this.rabbitMqOptions = rabbitMqOptions;
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
            PublishToMessageQueue(ExchaneEvents.UserAdd, GetUserDataExchange(value).ToString());
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
                PublishToMessageQueue(ExchaneEvents.UserUpdate, GetUserDataExchange(value).ToString());
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
                var userToBeDeleted = await appUserRepository.GetById(id);
                var deleted = await appUserRepository.Delete(id);
                if (deleted)
                {
                    PublishToMessageQueue(ExchaneEvents.UserDelete, GetUserDataExchange(userToBeDeleted).ToString());
                    return Ok("User deleted successfully.");
                }
                else
                    return Ok("User deletion failed.");

            }
            catch (EntityNotFoundException)
            {

                return NotFound($"The id {id} is not found.");
            }
        }


        private UserDataExchange GetUserDataExchange(AppUser user)
        {
            return new UserDataExchange
            {
                Name = $"{user.FirstName} {user.LastName}".Trim(),
                EmailId = user.EmailId,
                UserId = user.UserId
            };
        }
        private void PublishToMessageQueue(string routingKey, string eventData)
        {

            string hostname = rabbitMqOptions.Value.Hostname;
            string userName = rabbitMqOptions.Value.UserName;
            string password = rabbitMqOptions.Value.Password;
            string exchangeName = rabbitMqOptions.Value.ExchangeName;
            Publisher publisher = new Publisher(hostname, userName, password);

            publisher.PublishToMessageQueue(exchangeName, routingKey, eventData);

           
        }
    }
}
