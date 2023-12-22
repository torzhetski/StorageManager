using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Storage_Manager
{
    public class IntegerValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {

            if (uint.TryParse(value?.ToString(), out uint result))
            {
                return ValidationResult.ValidResult;
            }
            else
            {
                MessageBox.Show("You can only enter positive integer");
                return new ValidationResult(false, "Enter positive integer");
            }
        }
    }
}
