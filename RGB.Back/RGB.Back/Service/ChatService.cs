using Microsoft.EntityFrameworkCore;
using RGB.Back.DTOs;
using RGB.Back.Models;

namespace RGB.Back.Service
{
    public class ChatService
    {
        private readonly RizzContext _context;

        public ChatService(RizzContext context)
        {
            _context = context;
        }

        public List<UserInfoDto> GetAllFriends(int id)
        { 
            var friends = _context.Friends.Where(f => f.Member1Id == id)
                                          .Join(_context.Members,
                                                f => f.Member2Id,
                                                m => m.Id,
                                                (f, m) => new UserInfoDto
                                                {
                                                    UserId = m.Id,
                                                    UserName = m.NickName,
                                                    LastLoginTime = m.LastLoginDate,
                                                    AvatarUrl = m.AvatarUrl
                                                }).ToList();
                                          
            
            return friends;
        }

        public async Task<List<ChatMessageDto>> GetMessageHistory(int memberId, int friendId)
        {
            var messages = await _context.ChatMessages.Where(m => (m.SenderId == memberId && m.ReceiveId == friendId) || (m.SenderId == friendId && m.ReceiveId == memberId))
                                                .OrderBy(m => m.Time)
                                                .Select(m => new ChatMessageDto
                                                {
                                                    Id = m.Id,
                                                    sender_id = m.SenderId,
                                                    receiver_id = m.ReceiveId,
                                                    Content = m.Content,
                                                    Time = m.Time,
                                                    isRead = m.Isread
                                                }).ToListAsync();

            return messages;
        }
        internal object SendMessage(string data)
        {
            return "傳送訊息" + data;
        }

        internal object SendCaller(int senderId, int receiveId, string data)
        {
            var messageDto = new ChatMessageDto
            {
             
                sender_id = senderId,
                receiver_id = receiveId,
                Content = data,
                Time = DateTime.Now,
                isRead = 1
            };
            return messageDto;
        }

        internal object SendMessageToFriend(int senderId, int receiveId, string data)
        {
            var message = new ChatMessage
            {
                SenderId = senderId,
                ReceiveId = receiveId,
                Content = data,
                Time = DateTime.Now,
                Isread = 1
            };

            _context.ChatMessages.Add(message);
            _context.SaveChanges();

         
            return message;
        }
    }
}
