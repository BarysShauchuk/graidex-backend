using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Graidex.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("test-student"), Authorize(Roles = "Student")]
        public async Task<ActionResult<string>> TestStudentLogin()
        {
            return Ok("Hello, Student!");
        }

        [HttpGet("test-teacher"), Authorize(Roles = "Teacher")]
        public async Task<ActionResult<string>> TestTeacherLogin()
        {
            return Ok("Hello, Teacher!");
        }
    }
}
