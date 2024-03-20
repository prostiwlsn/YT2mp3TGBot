using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YT2mp3.Models;
using YT2mp3.Services;

namespace YT2mp3.Controllers
{
    [Route("")]
    [ApiController]
    public class BotController : ControllerBase
    {
        private readonly ILogger<BotController> _logger;
        private readonly IMessageHandler _handler;

        public BotController(ILogger<BotController> logger, IMessageHandler messageHandler)
        {
            _logger = logger;
            _handler = messageHandler;
        }
        [HttpPost]
        public async Task<IActionResult> Update([FromBody] Update update)
        {
            Console.WriteLine("recieved");
            Console.WriteLine(update);
            _handler.HandleMessage(update);
            return Ok();
        }
    }
}
