using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Course.Validation
{
    class MinimumCharacterRule : ValidationRule
    {
        public int MinimumCharacters { get; set; }

        
        Regex r = new Regex(@"^$|^[а-яА-Яa-zA-Z\s]+$"); 
        
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string charString = value as string;

            if (string.IsNullOrWhiteSpace(charString))
                return new ValidationResult(false, $"Поле не может быть пустым");

            if (charString.Length < MinimumCharacters)
                return new ValidationResult(false, $"Поле должно содержать более {MinimumCharacters} символов.");
            if (r.IsMatch(charString) != true)
            {
                return new ValidationResult(false, $"Поле содержит недопустимые символы");
            }

                return new ValidationResult(true, null);
        }
    }
}
