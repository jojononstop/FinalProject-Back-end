namespace RGB.Back.DTOs
{
    public class FriendRequestDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiveId { get; set; }

        public string SenderName { get; set; }
        public string SenderAvatarUrl { get; set; }
    }
}
