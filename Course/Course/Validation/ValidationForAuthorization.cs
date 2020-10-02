using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Course.Validation
{
    class ValidationForAuthorization : ValidationRule
    {
        public int MinimumCharacters { get; set; }


        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string charString = value as string;

            if (string.IsNullOrWhiteSpace(charString))
                return new ValidationResult(false, $"Поле не может быть пустым");

            if (charString.Length < MinimumCharacters)
                return new ValidationResult(false, $"Поле должно содержать более {MinimumCharacters} символов.");

            return new ValidationResult(true, null);
        }
    }
}
