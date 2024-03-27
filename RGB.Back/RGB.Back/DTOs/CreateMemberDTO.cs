namespace RGB.Back.DTOs
{
	public class CreateMemberDTO
	{
		public string Account { get; set; }
		public string Password { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string? Google { get; set; }
	}
}
