using Siedlisko.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siedlisko.Models
{
    public class Price
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public PriceFor PriceFor { get; set; }
    }
}
