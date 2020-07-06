// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace SBTP.View.XGYC
{
    public class AgeRangeRule : ValidationRule
    {
        public int Min { get; set; }
        public int Max { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var age = 0;

            try
            {
                if (((string) value).Length > 0)
                    age = int.Parse((string) value);
            }
            catch (Exception e)
            {
                MessageBox.Show("Illegal characters or " + e.Message);
                return new ValidationResult(false, "Illegal characters or " + e.Message);
                
            }

            if ((age < Min) || (age > Max))
            {
                MessageBox.Show("Please enter an age in the range: " + Min + " - " + Max + ".");
                return new ValidationResult(false,
                    "Please enter an age in the range: " + Min + " - " + Max + ".");
            }
            return new ValidationResult(true, null);
        }
    }
}