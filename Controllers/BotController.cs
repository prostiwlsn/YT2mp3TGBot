using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace YT2mp3.Controllers
{
    [Route("")]
    [ApiController]
    public class BotController : ControllerBase
    {
        private readonly ILogger<BotController> _logger;

        public BotController(ILogger<BotController> logger)
        {
            _logger = logger;
        }
        [HttpPost]
        public async Task<IActionResult> Update([FromBody] Object update)
        {
            // Handle the update here
            Console.WriteLine("webhook working as intended");
            return Ok();
        }
    }
}
