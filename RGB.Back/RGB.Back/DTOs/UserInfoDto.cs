using Microsoft.AspNetCore.Mvc;

namespace RGB.Back.DTOs
{
    public  class UserInfoDto 
    {
        public  int? UserId { get; set; }
        public  string? ConnectionId { get; set; }
        public  string? UserName { get; set; }
        public  string? AvatarUrl { get; set; }
        public  DateTime? LastLoginTime { get; set; }
    }
}
