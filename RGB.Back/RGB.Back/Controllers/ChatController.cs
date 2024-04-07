using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RGB.Back.DTOs;
using RGB.Back.Hubs;
using RGB.Back.Interfaces;
using RGB.Back.Models;
using RGB.Back.Service;

namespace RGB.Back.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        readonly ILogger<ChatController> _logger;
        readonly ChatService _service;

        public ChatController(ILogger<ChatController> logger, ChatService chatService)
        {
            _logger = logger;
            _service = chatService;
        }


        [HttpGet("GetUserFriends", Name = "GetUserFriends")]
        public async Task<List<UserInfoDto>> GetUserFriends(int id)
        {
            var friends = _service.GetAllFriends(id);
            var onlineUsers = ChatHub.OnlineUsers;
            foreach (var friend in friends)
            {
                var onlineUser = onlineUsers.Find(u => u.UserId == friend.UserId);
                if (onlineUser != null)
                {
                    friend.ConnectionId = onlineUser.ConnectionId;
                }
            }
            return friends;
        }

        [HttpGet("IsUserFriend", Name = "IsUserFriend")]
        public async Task<int> IsUserFriend(int memberId, int friendId)
        {
            var isFriend = await _service.IsFriend(memberId, friendId);
            return isFriend;
        }

        [HttpGet("GetMemberId", Name = "GetMemberId")]
        public async Task<int> GetMemberId(string userName)
        {
            var id = await _service.GetFriendId(userName);
            return id;

        }

        [HttpGet("SendMessageTo", Name = "SendMessageTo")]
        public async Task<IActionResult> SendMessageTo(string connectionId, string data, [FromServices] IHubContext<ChatHub, IChatClient> hubContext)
        {
            await hubContext.Clients.Client(connectionId).SendMessageTo(data);
            return Ok("Send Successful!");
        }
        [HttpGet("GetMessageHistory", Name = "GetMessageHistory")]
        public async Task<List<ChatMessageDto>> GetMessageHistory(int memberId, int friendId)
        {
            var messages = await _service.GetMessageHistory(memberId, friendId);
            return messages;
        }

        [HttpPost("AddFriendRequest", Name = "AddFriendRequest")]
        public async Task<IActionResult> AddFriendRequest(int memberId, int friendId)
        {

            await _service.AddFriend(memberId, friendId);

            return Ok("已發送好友邀請");
        }

        [HttpGet("GetFriendRequests", Name = "GetFriendRequests")]
        public async Task<List<FriendRequestDto>> GetFriendRequests(int memberId)
        {
            var requests = await _service.GetFriendRequests(memberId);
            return requests;
        }

        [HttpPut("Accept")]
        public async Task<IActionResult> AcceptFriendRequest(int senderId, int receiveId)
        {
            await _service.AcceptFriendRequest(senderId, receiveId);
            return Ok("已接受好友邀請");
        }

        [HttpPut("Reject")]
        public async Task<IActionResult> RejectFriendRequest(int senderId, int receiveId)
        {
            await _service.RejectFriendRequest(senderId, receiveId);
            return Ok("已拒絕好友邀請");
        }




    }
}
