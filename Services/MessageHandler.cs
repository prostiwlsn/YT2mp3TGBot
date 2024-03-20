using Microsoft.Extensions.Configuration;
using YT2mp3.Models;

namespace YT2mp3.Services
{
    public class MessageHandler : IMessageHandler
    {
        private readonly IConfiguration _configuration;
        public MessageHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task HandleMessage(Update update)
        {
            using (HttpClient http = new HttpClient())
            {
                string token = _configuration.GetValue<string>("BotToken")!;
                string url = $"https://api.telegram.org/bot{token}/sendMessage?chat_id={update.message.chat.id}&text=Принято в обработку";

                HttpContent content = new StringContent("");
                var response = await http.PostAsync(url, content);
                Console.WriteLine(response.StatusCode);
            }
        }
        public async Task SendMessage(Message message)
        {
            //TODO: send actual message
        }
    }
}
