using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiedliskoCommon.DataValidators
{
    public class IsNotZeroOrLess : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return int.Parse(value.ToString()) > 0;
        }
    }
}
