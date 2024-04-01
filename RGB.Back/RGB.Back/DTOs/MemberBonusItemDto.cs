namespace RGB.Back.DTOs
{
    public class MemberBonusItemDto
    {
        public int Id { get; set; }

        public int MemberId { get; set; }

        public int BonusId { get; set; }

        public int ProductType { get; set; }

        public bool Using { get; set; }
    }
}
