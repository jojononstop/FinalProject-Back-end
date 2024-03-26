namespace RGB.Back.DTOs
{
    public class OdersDTO
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
    
        public DateTime OrderDate { get; set; }

        public string PaymentMethod { get; set; }

        public int TotalAmount { get; set; }

        public int? DiscountAmount { get; set; }

        public string Status { get; set; }
        public string Message { get; set; }
    }
}
