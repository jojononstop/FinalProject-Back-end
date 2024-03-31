using RGB.Back.Models;
using System.ComponentModel.DataAnnotations;

namespace RGB.Back.DTOs
{
	public class GameDetailDTO
	{
		public int Id { get; set; }

		public int DeveloperId { get; set; }

		public string? Name { get; set; }

		public string? Introduction { get; set; }

		public string? Description { get; set; }

		public DateTime ReleaseDate { get; set; }

		public int? Price { get; set; }

		public string? Cover { get; set; }

		//public int? MaxPercent { get; set; }

		public List<string>? DisplayImages { get; set; }



		public string? Video { get; set; }

		public List<Discount>? Discounts { get; set; }

		public double Rating { get; set; }

		public int DiscountPercent { get; set; }

		public int DiscountPrice { get; set; }
		public List<DlcDetailDTO>? DLCs { get; set; }
		public List<Tag>? Tags { get; set; }
	}

	public class DlcDetailDTO
	{
		public int Id { get; set; }

		public int DeveloperId { get; set; }

		public string? Name { get; set; }

		public string? Introduction { get; set; }

		public string? Description { get; set; }

		public DateTime ReleaseDate { get; set; }

		public int? Price { get; set; }

		public string? Cover { get; set; }

		//public int? MaxPercent { get; set; }

		public List<string>? DisplayImages { get; set; }

		public List<Tag>? Tags { get; set; }

		public string? Video { get; set; }

		public List<Discount>? Discounts { get; set; }

		public double Rating { get; set; }

		public int DiscountPercent { get; set; }

		public int DiscountPrice { get; set; }

	}
}
