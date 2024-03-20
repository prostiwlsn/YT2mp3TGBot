namespace YT2mp3.Models
{
    public class Message
    {
        public int message_id { get; set; }
        public int date { get; set; }
        public string text { get; set; }
        public User from { get; set; }
        public Chat chat { get; set; }
    }
}
