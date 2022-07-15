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
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentsService _studentsService;
        private readonly ILogger<StudentsController> _logger;
        public StudentsController(ILogger<StudentsController> logger, IStudentsService studentsService)
        {
            this._logger = logger;
            this._studentsService = studentsService;
        }
        [HttpGet("{name}")]
        public IActionResult Get(string name)
        {
            _logger.LogInformation($"Get by name: {name} in Students Controller");
            Students students = _studentsService.GetByName(name);
            if (students != null)
            {
                _logger.LogInformation($"There is data in DB and it will be returned");
                return StatusCode((int)HttpStatusCode.OK, students);
            }
            _logger.LogInformation($"There is no data in DB and it will not be returned");
            return StatusCode((int)HttpStatusCode.NotFound);
        }
        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Get in Students Controller");
            DbSet<Students> users = _studentsService.Get();
            if (users != null && users.ToList().Count > 0)
            {
                _logger.LogInformation($"There is data in DB and it will be returned");
                return StatusCode((int)HttpStatusCode.OK, users);
            }
            _logger.LogInformation($"There is no data in DB and it will not be returned");
            return StatusCode((int)HttpStatusCode.NotFound);
        }
        [HttpPost]
        public IActionResult Post([FromBody] Students students)
        {
            try
            {
                if (students != null)
                {
                    _logger.LogInformation("Adding a new student in Students Controller");
                    Guid id = _studentsService.Post(students);
                    _logger.LogInformation("New student was adding in Students Controller");
                    return StatusCode((int)HttpStatusCode.Created, Constants.CreateStudentsMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"An Exception ocurred in Students Controller for student: {students.ToString()}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
            _logger.LogInformation("Student is null");
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
        [HttpPut]
        public IActionResult Put([FromBody] Students students)
        {
            try
            {
                if (students != null)
                {
                    _logger.LogInformation("Modifying a student in Students Controller");
                    _studentsService.Put(students);
                    _logger.LogInformation("Student was modified in Students Controller");
                    return StatusCode((int)HttpStatusCode.Accepted, Constants.UpdateStudentsMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"An Exception ocurred in Students Controller for student: {students.ToString()}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
            _logger.LogInformation("Student is null");
            return StatusCode((int)HttpStatusCode.NotFound);
        }
        [HttpDelete]
        public IActionResult Delete([FromBody] Guid id)
        {
            try
            {
                if (id != Guid.Empty)
                {
                    _logger.LogInformation("Deleting a student in Students Controller");
                    _studentsService.Delete(new Students() { Id = id });
                    _logger.LogInformation("Student was deleted in Students Controller");
                    return StatusCode((int)HttpStatusCode.OK, Constants.DeleteStudentsMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"An Exception ocurred in Students Controller for Id: {id}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
            _logger.LogInformation("Id is null");
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
