using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BlogClient.Controllers
{
    public class BaseController : Controller
    {
        protected IActionResult HandleApiError(HttpStatusCode statusCode)
        {
            switch (statusCode)
            {
                case HttpStatusCode.NotFound:
                    return NotFound();
                case HttpStatusCode.Unauthorized:
                    return Unauthorized();
                case HttpStatusCode.BadRequest:
                    return BadRequest();
                case HttpStatusCode.Forbidden:
                    return Forbid();
                default:
                    return StatusCode((int)statusCode);
            }
        }
    }
}
