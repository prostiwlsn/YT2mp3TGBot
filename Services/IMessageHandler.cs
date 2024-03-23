using YT2mp3.Models;

namespace YT2mp3.Services
{
    public interface IMessageHandler
    {
        Task HandleMessage(UpdateInfo message);
    }
}
