using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using RGB.Back.DTOs;
using RGB.Back.Interfaces;
using RGB.Back.Service;

namespace RGB.Back.Hubs
{
    public class ChatHub : Hub<IChatClient>
    {
        ILogger<ChatHub> _logger;
        readonly ChatService _service;

        public static List<UserInfoDto> OnlineUsers = new List<UserInfoDto>();
        public ChatHub(ILogger<ChatHub> logger, ChatService service)
        {
            _logger = logger;
            _service = service;
        }

        //客戶端連接服務端時
        public override async Task OnConnectedAsync()
        {

            var id = Context.ConnectionId;

            var userId = Context.GetHttpContext().Request.Query["userId"];

            _logger.LogInformation($"客戶端id={id},連接服務端,使用者 ID={userId}");

            // 取得上線的使用者
            var onlineUser = new UserInfoDto 
            {
                UserId = int.Parse(userId),
                ConnectionId = id,
                LastLoginTime = DateTime.Now
                
            }; // 假設 UserInfoDto 有一個 ConnectionId 屬性表示連接ID
            OnlineUsers.Add(onlineUser);

            // 更新其他使用者的上線資訊
            await Clients.Others.UserConnected(onlineUser);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var id = Context.ConnectionId;
           

            var offlineUser = OnlineUsers.FirstOrDefault(u => u.ConnectionId == id);

            OnlineUsers.Remove(offlineUser);

            await Clients.Others.UserDisconnected(offlineUser);


            await base.OnDisconnectedAsync(exception);
        }


        public async Task SendMessageToFriend(int senderId, int receiveId, string friendConnectionId ,string message)
        {
           
            await Clients.Caller.SendMessageTo(_service.SendCaller(senderId, receiveId, message));
            await Clients.Client(friendConnectionId).SendMessageTo(_service.SendMessageToFriend(senderId, receiveId, message));
        }



    }
}
