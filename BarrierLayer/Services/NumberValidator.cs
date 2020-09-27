using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BarrierLayer.Services
{
    public static class NumberValidator
    {
        public static bool ValidateNumber(this string number)
        {
            var regex = new Regex(@"^\+?7?\(?\d{3}\)?\d{3}\-?\d{2}\-?\d{2}$");
            return regex.IsMatch(number);
        }
        public static string FormatToNumber(this string number)
        {
            if (!number.ValidateNumber()) return null;
            number = number.Replace("-","");
            number = number.Replace("(", "");
            number = number.Replace(")", "");
            number = number.Replace("+", "");
            if (number.Length == 10) number = "7" + number;
            return number;
        }
    }
}
