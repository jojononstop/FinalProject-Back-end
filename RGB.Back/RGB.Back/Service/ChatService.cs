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
        internal object SendMessage(string data)
        {
            return data;
        }

        internal object SendCaller(object data)
        {
            return data;
        }
    }
}
