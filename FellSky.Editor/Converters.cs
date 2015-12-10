﻿using FellSky.Graphics;
using System;
using System.Globalization;
using XnaColor = Microsoft.Xna.Framework.Color;
using WpfColor = System.Windows.Media.Color;

namespace FellSky.Editor
{
    public class SpriteToIntRectConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var sprite = value as JsonSprite;
            if (sprite == null) return null;
            return new System.Windows.Int32Rect(sprite.X, sprite.Y, sprite.W, sprite.H);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class XnaColorToBrushConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is XnaColor)) return null;
            var color = (XnaColor)value;
            return new System.Windows.Media.SolidColorBrush(WpfColor.FromArgb(color.A, color.R, color.G, color.B));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var brush = value as System.Windows.Media.SolidColorBrush;
            if (brush == null) return null;
            return new XnaColor(brush.Color.R, brush.Color.G, brush.Color.B, brush.Color.A);
        }
    }

    public class ContrastingColorConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is WpfColor))
                return value;
            WpfColor color = (WpfColor)value;
            double Y = 0.2126 * color.ScR + 0.7152 * color.ScG + 0.0722 * color.ScB;
            return Y > 0.4 ? System.Windows.Media.Brushes.Black : System.Windows.Media.Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}