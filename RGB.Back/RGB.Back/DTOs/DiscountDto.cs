namespace RGB.Back.DTOs
{
    public class DiscountDto
    {
        public int Id { get; set; }
        public DateOnly StartTime { get; set; }
        public DateOnly EndTime { get; set; }
        public int Percent { get; set; }
        public string DiscountType { get; set; }
        public string DiscountName { get; set; }
        public string DiscountDescription { get; set;}
        public string Image { get; set; }
    }
}
