using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Course.Validation
{
    class HomeRule : ValidationRule
    {
        Regex r = new Regex(@"[^0-9]");

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string charString = value as string;

            if (r.IsMatch(charString) == true)
            {
                return new ValidationResult(false, $"Поле содержит недопустимые символы");
            }

            return new ValidationResult(true, null);
        }
    }
}
