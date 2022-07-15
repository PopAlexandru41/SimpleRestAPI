using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleRestAPI.Models;
using SimpleRestAPI.Services.Interface;
using System.Net;

namespace SimpleRestAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            this._logger = logger;
            this._userService = userService;
        }
        [HttpGet("{name}")]
        public IActionResult Get(string name)
        {
            _logger.LogInformation($"Get by name: {name} in User Controller");
            User user = _userService.GetByName(name);
            if (user != null)
            {
                _logger.LogInformation($"There is data in DB and it will be returned");
                return StatusCode((int)HttpStatusCode.OK, user);
            }
            _logger.LogInformation($"There is no data in DB and it will not be returned");
            return StatusCode((int)HttpStatusCode.NotFound);
        }
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Get in User Controller");
            DbSet<User> users = _userService.Get();
            if (users != null)
            {
                if (users.ToList().Count > 0)
                {
                    _logger.LogInformation($"There is data in DB and it will be returned");
                    return StatusCode((int)HttpStatusCode.OK, users);
                }
            }
            _logger.LogInformation($"There is no data in DB and it will not be returned");
            return StatusCode((int)HttpStatusCode.NotFound);
        }
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            try
            {
                if (user != null)
                {
                    _logger.LogInformation("Adding a new student in User Controller");
                    Guid id = _userService.Post(user);
                    _logger.LogInformation("New student was adding in User Controller");
                    return StatusCode((int)HttpStatusCode.Created, Constants.CreateUserMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"An Exception ocurred in User Controller for student: {user.ToString()}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
            _logger.LogInformation("Student is null");
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut]
        public IActionResult Put([FromBody] User user)
        {
            try
            {
                if (user != null)
                {
                    _logger.LogInformation("Modifying a student in User Controller");
                    _userService.Put(user);
                    _logger.LogInformation("Student was modified in User Controller");
                    return StatusCode((int)HttpStatusCode.Accepted, Constants.UpdateUserMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"An Exception ocurred in User Controller for student: {user.ToString()}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
            _logger.LogInformation("Student is null");
            return StatusCode((int)HttpStatusCode.NotFound);
        }
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete]
        public IActionResult Delete([FromBody] Guid id)
        {
            try
            {
                if (id != Guid.Empty)
                {
                    _logger.LogInformation("Deleting a student in User Controller");
                    _userService.Delete(new User() { Id = id });
                    _logger.LogInformation("Student was deleted in User Controller");
                    return StatusCode((int)HttpStatusCode.OK, Constants.DeleteUserMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"An Exception ocurred in User Controller for Id: {id}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
            _logger.LogInformation("Id is null");
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
