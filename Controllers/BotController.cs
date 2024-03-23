using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using YT2mp3.Models;
using YT2mp3.Services;
using System.Text.RegularExpressions;

namespace YT2mp3.Controllers
{
    [Route("")]
    [ApiController]
    public class BotController : ControllerBase
    {
        private readonly ILogger<BotController> _logger;
        private readonly IMessageHandler _handler;

        private Regex _textRegex = new Regex("(?<=\"text\":\")[^\"]*(?=\"(,|}))");
        private Regex _chatIdRegex = new Regex("(?<=\"chat\":{\"id\":)\\d+(?=,)");

        public BotController(ILogger<BotController> logger, IMessageHandler messageHandler)
        {
            _logger = logger;
            _handler = messageHandler;
        }
        [HttpPost]
        public async Task<IActionResult> Update([FromBody] Object update)
        {
            string updateString = update.ToString();
            Console.WriteLine(updateString);

            MatchCollection textMatches = _textRegex.Matches(updateString);
            MatchCollection idMatches = _chatIdRegex.Matches(updateString);

            if(textMatches.Count == 0 || idMatches.Count == 0)
                return NotFound();

            Console.WriteLine(textMatches[0]);
            Console.WriteLine(idMatches[0]);

            UpdateInfo updateInfo = new UpdateInfo();
            updateInfo.Text = textMatches[0].Value;
            updateInfo.ChatId = int.Parse(idMatches[0].Value); 

            _handler.HandleMessage(updateInfo);

            return Ok();
        }
    }
}
