using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http.Headers;
using VideoLibrary;
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

        public async Task HandleMessage(UpdateInfo update)
        {
            Task pendingMessage = SendMessage(update.ChatId, "In processing");
            string newFile = Guid.NewGuid().ToString();

            try
            {
                await GenerateFile(update.Text.Replace(" ", ""), newFile);
                await SendFile(update.ChatId, newFile);

                string folderPath = _configuration.GetValue<string>("FileDirectory")!;

                Console.WriteLine(folderPath + newFile);

                Converter.DeleteFile(folderPath + newFile + ".mp3");
                Converter.DeleteFile(folderPath + newFile + ".mp4");
            }
            catch (Exception ex)
            {
                await SendMessage(update.ChatId, ex.Message);
            }

            await pendingMessage;
            pendingMessage.Dispose();
        }
        public async Task SendFile(int chatId, string fileName)
        {
            using (HttpClient http = new HttpClient())
            {
                string token = _configuration.GetValue<string>("BotToken")!;
                string url = $"https://api.telegram.org/bot{token}/sendAudio?chat_id={chatId}";

                using (var multipartFormDataContent = new MultipartFormDataContent())
                {
                    string folderPath = _configuration.GetValue<string>("FileDirectory")!;
                    byte[] audioBytes = File.ReadAllBytes(folderPath+fileName+".mp3");

                    var streamContent = new StreamContent(new MemoryStream(audioBytes));
                    streamContent.Headers.ContentType = new MediaTypeHeaderValue("audio/mpeg");
                    streamContent.Headers.Add("Content-Disposition", $"form-data; name=\"audio\"; filename=\"{Path.GetFileName(folderPath + fileName + ".mp3")}\"");
                    multipartFormDataContent.Add(streamContent, "audio", Path.GetFileName(folderPath + fileName + ".mp3"));

                    using (var message = await http.PostAsync(url, multipartFormDataContent))
                    {
                        var contentString = await message.Content.ReadAsStringAsync();
                        Console.WriteLine(contentString);
                    }
                }
            }
        }

        public async Task SendMessage(int id, string message)
        {
            using (HttpClient http = new HttpClient())
            {
                string token = _configuration.GetValue<string>("BotToken")!;
                string url = $"https://api.telegram.org/bot{token}/sendMessage?chat_id={id}&text={message}";

                HttpContent content = new StringContent("");
                var response = await http.PostAsync(url, content);
                Console.WriteLine(response.StatusCode);

                content.Dispose();
            }
        }
        private async Task GenerateFile(string uri, string newFile)
        {
            string folderPath = _configuration.GetValue<string>("FileDirectory")!;
            string ffmpegPath = _configuration.GetValue<string>("ffmpegPath")!;

            await Converter.Convert(uri, folderPath, newFile, ffmpegPath);
        }
    }
}
