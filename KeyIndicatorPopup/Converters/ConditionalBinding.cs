using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;

namespace KeyIndicatorPopup.Converters
{
    //see: https://stackoverflow.com/questions/28817250/conditional-text-binding-xaml/28817452
    public class ConditionalBinding : IMultiValueConverter
    {
        /// <summary>
        /// Return value[1] if value[0] == true, return value[2] if value[0] == false
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToBoolean(values[0]) == true ? values[1] : values[2];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
