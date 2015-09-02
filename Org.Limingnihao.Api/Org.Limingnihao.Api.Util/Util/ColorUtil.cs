using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.Limingnihao.Api.Util
{
    public class ColorUtil
    {
        public static System.Windows.Media.Color StringToColor(string colorName)
        {
            if (colorName.StartsWith("#"))
            {
                colorName = colorName.Replace("#", string.Empty);
            }
            int v = int.Parse(colorName, System.Globalization.NumberStyles.HexNumber);
            byte a = 255;// Convert.ToByte((v >> 24) & 255);
            byte r = Convert.ToByte((v >> 16) & 255);
            byte g = Convert.ToByte((v >> 8) & 255);
            byte b = Convert.ToByte((v >> 0) & 255);
            return System.Windows.Media.Color.FromArgb(a, r, g, b);
        }

        public static System.Windows.Media.Brush StringToBrush(string colorName)
        {
            return (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString(colorName);
        }
    }
}
