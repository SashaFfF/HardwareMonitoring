using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareMonitoringUI.Converter
{
    public class PercentageConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int percentage)
            {
                return (double)percentage / 100.0;
            }

            return 0.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public int NullConvert()
        {
            Random random = new Random();
            int randomNumber = random.Next(3); // генерация случайного числа от 0 до 2
            int[] possibleNumbers = { 6, 8, 11 }; // массив возможных чисел
            return possibleNumbers[randomNumber]; 
        }
    }
}
