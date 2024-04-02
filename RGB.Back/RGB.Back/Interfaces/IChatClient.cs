using RGB.Back.DTOs;

namespace RGB.Back.Interfaces
{
    public interface IChatClient
    {
        Task SendMessage(object message);
        Task SendMessageTo(object message);
        Task UserConnected(UserInfoDto onlineUser);
        Task UserDisconnected(UserInfoDto offlineUser);
    }
}
