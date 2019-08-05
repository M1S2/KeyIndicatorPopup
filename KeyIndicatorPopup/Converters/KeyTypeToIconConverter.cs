using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Media;
using MahApps.Metro.IconPacks;
using KeyIndicatorPopup.Keyboard;

namespace KeyIndicatorPopup.Converters
{
    /// <summary>
    /// Convert KeyTypes enum values to GeometryDrawing representation of the key type icon
    /// </summary>
    [ValueConversion(typeof(KeyTypes), typeof(GeometryDrawing))]
    public class KeyTypeToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Geometry iconGeometry = null;
            Brush foregroundBrush = App.Current.TryFindResource("AccentBaseColorBrush") as Brush;

            switch ((KeyTypes)value)
            {
                case KeyTypes.Lock:
                    iconGeometry = Geometry.Parse((new PackIconEntypo() { Kind = PackIconEntypoKind.LockOpen }).Data); break;
                case KeyTypes.Letter:
                    iconGeometry = Geometry.Parse((new PackIconMaterial() { Kind = PackIconMaterialKind.Alphabetical }).Data); break;
                case KeyTypes.Numeric:
                    iconGeometry = Geometry.Parse((new PackIconMaterial() { Kind = PackIconMaterialKind.Numeric }).Data); break;
                case KeyTypes.System:
                    iconGeometry = Geometry.Parse((new PackIconEntypo() { Kind = PackIconEntypoKind.Keyboard }).Data); break;

                default:
                    return null;
            }

            GeometryDrawing iconGeometryDrawing = new GeometryDrawing(foregroundBrush, new Pen(foregroundBrush, 1), iconGeometry);
            return iconGeometryDrawing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
