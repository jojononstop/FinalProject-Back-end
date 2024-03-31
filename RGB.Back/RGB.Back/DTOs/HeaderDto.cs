using System.Reflection;

namespace RGB.Back.DTOs
{
    public class HeaderDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public SubMenuDto[] SubMenu { get; set; }

    }

    public class SubMenuDto
    {
        public string Title { get; set; }
        public string Link { get; set; }
    }
}
