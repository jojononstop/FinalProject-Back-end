using Microsoft.Build.ObjectModelRemoting;

namespace RGB.Back.DTOs
{
    public class ChatMessageDto
    {
        public int Id { get; set; }
        public int sender_id { get; set; }
        public int receiver_id { get; set; }
        public string Message { get; set; }
        public DateTime? SendTime { get; set; }
        public byte? isRead { get; set; }

    }
}
