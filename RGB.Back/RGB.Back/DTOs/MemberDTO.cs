namespace RGB.Back.DTOs
{
	public class MemberDTO
	{
		public int Id { get; set; }

		public string Account { get; set; }

		public string Password { get; set; }

		public string Mail { get; set; }

		public string? AvatarUrl { get; set; }

		public DateTime RegistrationDate { get; set; }

		public DateTime? BanTime { get; set; }

		public int Bonus { get; set; }

		public DateTime LastLoginDate { get; set; }

		public DateOnly? Birthday { get; set; }

		public string NickName { get; set; }
	}
}
