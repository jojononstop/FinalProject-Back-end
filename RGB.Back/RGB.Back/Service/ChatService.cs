using Microsoft.AspNetCore.Http.HttpResults;
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
        internal async Task<int> GetFriendId(string? name)
        {
            var Friendname = await _context.Members.FirstOrDefaultAsync(m => m.NickName == name);


            if (Friendname == null)
            {
                throw new Exception("查無此人");
            }

            return Friendname.Id;
        }

        internal async Task AddFriend(int senderId, int receiveId)
        {
            if (senderId == receiveId)
            {
                throw new Exception("不能添加自己为好友");
            }

            var existingRequset = await _context.FriendRequests.FirstOrDefaultAsync(r => r.SenderId == senderId && r.ReceiveId == receiveId);
            if (existingRequset != null)
            {
                throw new Exception("已经發送過好友请求");
            }

            var existingFriend = await _context.Friends.FirstOrDefaultAsync(f => (f.Member1Id == senderId && f.Member2Id == receiveId) || (f.Member1Id == receiveId && f.Member2Id == senderId));
            if (existingFriend != null)
            {
                throw new Exception("已经是好友");
            }

            var request = new FriendRequest();
            request.SenderId = senderId;
            request.ReceiveId = receiveId;

            _context.FriendRequests.Add(request);
            await _context.SaveChangesAsync();

        }

        internal async Task<List<FriendRequestDto>> GetFriendRequests(int memberId)
        {
            var requsets = await _context.FriendRequests.Where(r => r.ReceiveId == memberId)
                                                         .Join(_context.Members, r => r.SenderId, m => m.Id,
                                                               (r, m) => new FriendRequestDto
                                                               {
                                                                   Id = r.Id,
                                                                   SenderId = r.SenderId,
                                                                   ReceiveId = r.ReceiveId,
                                                                   SenderName = m.NickName,
                                                                   SenderAvatarUrl = m.AvatarUrl
                                                               }).ToListAsync();


            return requsets;
        }

        internal async Task AcceptFriendRequest(int senderId, int receiveId)
        {
            var requset = await _context.FriendRequests.FirstOrDefaultAsync(r => r.SenderId == senderId && r.ReceiveId == receiveId);
            if (requset == null)
            {
                throw new Exception("好友请求不存在");
            }

            _context.FriendRequests.Remove(requset);
            await _context.SaveChangesAsync();

            var friend = new Friend
            {
                Member1Id = senderId,
                Member2Id = receiveId,
                Relationship = "1",

            };

            var friend2 = new Friend
            {
                Member1Id = receiveId,
                Member2Id = senderId,
                Relationship = "1",

            };

            _context.Friends.Add(friend);
            _context.Friends.Add(friend2);
            await _context.SaveChangesAsync();
        }

        internal async Task RejectFriendRequest(int senderId, int receiveId)
        {
            var requset = await _context.FriendRequests.FirstOrDefaultAsync(r => r.SenderId == senderId && r.ReceiveId == receiveId);
            if (requset == null)
            {
                throw new Exception("好友请求不存在");
            }

            _context.FriendRequests.Remove(requset);
            await _context.SaveChangesAsync();
        }

        internal async Task<int> IsFriend(int memberId, int friendId)
        {
            var friend = await _context.Friends.FirstOrDefaultAsync(f => (f.Member1Id == memberId && f.Member2Id == friendId) || (f.Member1Id == friendId && f.Member2Id == memberId));
            if (friend == null)
            {
                return 0;
            }
            return 1;
        }
    }
}
