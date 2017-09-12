using Siedlisko.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siedlisko.Models.Helper
{
    public class PriceCounter
    {
        #region Fields and properties
        private SiedliskoContext _context;
        #endregion

        #region ctor

        public PriceCounter(SiedliskoContext context)
        {
            _context = context;
        }
        #endregion

        #region Public Api
        public decimal GetTotalCost(int Adults, int numberOfAccomodations)
        {
            var Cost = _context.Prices.FirstOrDefault(x => x.PriceFor == PriceFor.Room).Value;
            if (Adults > 4)
            {
                Cost += _context.Prices.FirstOrDefault(x => x.PriceFor == PriceFor.Adult).Value * (Adults - 4);
            }
            return Cost;
        }
        #endregion
    }
}
