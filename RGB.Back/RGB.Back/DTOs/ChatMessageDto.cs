using Microsoft.Build.ObjectModelRemoting;

namespace RGB.Back.DTOs
{
    public class ChatMessageDto
    {
        public int Id { get; set; }
        public int sender_id { get; set; }
        public int receiver_id { get; set; }
        public string Content { get; set; }
        public DateTime? Time { get; set; }
        public byte? isRead { get; set; }

    }
}
