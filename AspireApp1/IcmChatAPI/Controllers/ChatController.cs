using Microsoft.AspNetCore.Mvc;

namespace IcmChatAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        [HttpGet] // <-- Add this line right here!
        public IActionResult Ping()
        {
            return Ok("Pong");
        }
    }
}