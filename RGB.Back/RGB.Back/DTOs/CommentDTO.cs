using RGB.Back.Models;

namespace RGB.Back.DTOs
{
	public class CommentDTO
	{
		public int Id { get; set; }

		public int MemberId { get; set; }

		public int GameId { get; set; }

		public int Rating { get; set; }

		public string Comment1 { get; set; }

		public DateTime? Date { get; set; }

		public List<AttachedComment> AttachedComment { get; set; }

		public virtual Game Game { get; set; }

		public virtual Member Member { get; set; }

	}
}
