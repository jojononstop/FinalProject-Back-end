namespace RGB.Back.DTOs
{
    public class BonusDto
    {
        public int Id { get; set; }

        public int ProductTypeId { get; set; }

        public string ProductTypeName { get; set; }//JOIN BonusProductType.Name

        public int Price { get; set; }

        public string URL { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
