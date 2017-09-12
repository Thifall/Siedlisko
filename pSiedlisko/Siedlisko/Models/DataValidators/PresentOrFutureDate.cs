using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siedlisko.Models.DataValidators
{
    public sealed class PresentOrFutureDate : ValidationAttribute
    {
        #region public API
        public override bool IsValid(object value)
        {
            var date = (DateTime)value;
            if (date == null || date.Date < DateTime.Now.Date)
            {
                return false;
            }
            return true;
        } 
        #endregion
    }
}
