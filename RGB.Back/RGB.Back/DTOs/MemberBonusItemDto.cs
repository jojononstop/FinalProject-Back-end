namespace RGB.Back.DTOs
{
    public class MemberBonusItemDto
    {
        public int Id { get; set; }

        public int ProductTypeId { get; set; }//JOIN BonusProduc.Id

        public string ProductTypeName { get; set; }//JOIN BonusProductType.Name

        public int Price { get; set; }//JOIN BonusProduct.Price

        public string URL { get; set; }//JOIN BonusProduc.URL

        public string Name { get; set; }////JOIN BonusProduc.Name

        public int MemberId { get; set; }

        public int BonusId { get; set; }

        public int ProductType { get; set; }

        public bool Using { get; set; }
    }
}
