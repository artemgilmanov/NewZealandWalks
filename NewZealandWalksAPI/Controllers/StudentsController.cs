using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NewZealandWalksAPI.Controllers
{
    // https://localhost:portnumber/api/students
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        // GET: https://localhost:portnumber/api/students
        [HttpGet]
        public IActionResult GetAllStudent()
        {
            string[] studentNames = new string[] { "John", "Jane", "Mark", "Emily", "David" };
            return Ok(studentNames);
        }
    }
}
