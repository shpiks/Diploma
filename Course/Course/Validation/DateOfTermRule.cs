using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Course.Validation
{
    class DateOfTermRule : ValidationRule
    {
        public DateTime Registrationdate { get; set; }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {

            if (value != null)
            {
                DateTime date = (DateTime)value;
                string dateS = date.ToString();


                if (string.IsNullOrWhiteSpace(dateS))
                    return new ValidationResult(false, $"Поле не может быть пустым");

                if (date < DateTime.Today)
                    return new ValidationResult(false, $"Неверная дата регистрации");



                return new ValidationResult(true, null);
            }


            return new ValidationResult(false, $"Поле не может быть пустым");
        }
    }
}
