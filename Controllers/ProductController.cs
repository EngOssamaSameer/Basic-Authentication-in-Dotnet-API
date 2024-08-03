using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BasicAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        [HttpGet("6")]
        [Authorize(Roles ="Admin")]
        public IActionResult GetAdmin()
        {
            var username = HttpContext.User.Identity.Name;

            return Ok($"Done here is Admin DashBoard:{username}");
        }
        [HttpGet("7")]
        [Authorize(Roles ="User")]
        public IActionResult GetUser()
        {
            var username = HttpContext.User.Identity.Name;

            return Ok($"Done here is User Page:{username}");
        }

    }
}
