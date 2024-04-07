namespace RGB.Back.DTOs
{
	public class CollectionGameDTO
	{
		public int Id { get; set; }
		public int DeveloperId { get; set; }
		public string? Name { get; set; }
		public string? Cover { get; set; }
		public List<DlcCollectionDTO> Dlcs { get; set; }
	}
}
