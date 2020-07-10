using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SBTP.Common
{
    class DataFormatRule: ValidationRule
    {
        public double JCJJ_RADIUS { set; get; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var dataValue = 0;

            try
            {
                if (((string)value).Length > 0)
                    dataValue = int.Parse((string)value);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return new ValidationResult(false, e.Message);
            }

            if (dataValue> JCJJ_RADIUS)
            {
                MessageBox.Show("Please enter an age in the range: 0 - " + JCJJ_RADIUS + ".");
                return new ValidationResult(false,"Please enter an age in the range: 0 - " + JCJJ_RADIUS + ".");
            }
            return new ValidationResult(true, null);
        }
    }
}
