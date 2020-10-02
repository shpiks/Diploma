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
    class CityAndStreetRule : ValidationRule
    {
        public int MinimumCharacters { get; set; }


        Regex r = new Regex(@"^$|^[а-яА-Яa-zA-Z\s]+$");

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string charString = value as string;

            if (r.IsMatch(charString) != true)
            {
                return new ValidationResult(false, $"Поле содержит недопустимые символы");
            }

            return new ValidationResult(true, null);
        }
    }
}
