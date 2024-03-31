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

        [HttpGet("GetAllUsersIds", Name = "GetAllUserIds")]
        public List<UserInfoDto> GetAllUserIds()
        {
            return ChatHub.OnlineUsers;
        }

        [HttpGet("GetUserFriends", Name = "GetUserFriends")]
        public async Task<List<UserInfoDto>> GetUserFriends(int id)
        {
            var friends =  _service.GetAllFriends(id);
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

    }
}
