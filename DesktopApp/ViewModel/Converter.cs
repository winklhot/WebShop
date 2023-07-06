using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ShopBase;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using Microsoft.Win32;
using System.IO;

namespace DesktopApp
{
    public class Converter : IValueConverter
    {
        // Convert for UI
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object o = null;


            if (value != null)
            {
                switch (value)
                {
                    case decimal:
                        o = (decimal)value < 9999.98m ? value.ToString().Replace(".", ",") : "";
                        break;
                    case Article:
                        o = true;
                        break;
                    case Picture:
                        o = GetBitmapImage(value as Picture);
                        break;
                    case Customer:
                        o = true;
                        break;
                    case List<Position>:
                        List<Position> list = value as List<Position>;
                        o = list.Sum(x => x.Totalsum);
                        break;
                    case Order:
                        o = value != null;
                        break;
                    case ObservableCollection<Status>:
                        o = value as ObservableCollection<Status> != null;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                o = false;
            }

            return o;
        }

        // Convert from UI
        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object o = null;

            switch (value)
            {
                case string:
                    try
                    {
                        o = System.Convert.ToDecimal(value);
                    }
                    catch (Exception)
                    {
                        o = "";
                    }
                    break;
                default:
                    break;
            }

            return o;

        }


        public BitmapImage? GetBitmapImage(Picture p)
        {

            if (p == null || p.Data == null)
                return null;

            MemoryStream ms = new MemoryStream();

            BitmapImage bitmap = new BitmapImage();
            ms.Write(p.Data, 0, p.Data.Length);
            ms.Seek(0, SeekOrigin.Begin);
            bitmap.BeginInit();
            bitmap.StreamSource = ms;
            bitmap.EndInit();

            return bitmap;
        }
    }
}
