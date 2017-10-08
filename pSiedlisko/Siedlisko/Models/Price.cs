using SiedliskoCommon.Models.Enums;

namespace Siedlisko.Models
{
    public class Price
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public PriceFor PriceFor { get; set; }
    }
}
