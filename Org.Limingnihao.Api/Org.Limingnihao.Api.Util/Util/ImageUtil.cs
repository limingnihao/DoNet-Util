using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Org.Limingnihao.Api.Util
{
    /// <summary>
    /// 图片处理方法
    /// </summary>
    public class ImageUtil
    {
        public static void MakePicture(string path, string text)
        {
            BitmapSource bgImage = new BitmapImage(new Uri(path, UriKind.Absolute));

            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();


            drawingContext.DrawImage(bgImage, new Rect(0, 0, bgImage.Width, bgImage.Height));


            //计算签名的位置
            FormattedText signatureTxt = new FormattedText(text,
                                       System.Globalization.CultureInfo.CurrentCulture,
                                       System.Windows.FlowDirection.LeftToRight,
                                       new Typeface(System.Windows.SystemFonts.MessageFontFamily, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal),
                                       50,
                                       System.Windows.Media.Brushes.White);
            drawingContext.DrawText(signatureTxt, new Point(100, 100));
            drawingContext.DrawEllipse(Brushes.SaddleBrown, null, new Point(100, 100), 100, 100);
            drawingContext.Close();
            drawingContext = null;

            RenderTargetBitmap composeImage = new RenderTargetBitmap(bgImage.PixelWidth, bgImage.PixelHeight, bgImage.DpiX, bgImage.DpiY, PixelFormats.Default);
            composeImage.Render(drawingVisual);
            System.Console.WriteLine("DpiX=" + composeImage.DpiX + ", DpiY=" + composeImage.DpiY);

            //保存图片为png
            FileStream stream = new FileStream(@"d:\111111111111.jpg", FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write);
            //PngBitmapEncoder encoder = new PngBitmapEncoder();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(composeImage));
            encoder.Save(stream);
            stream.Flush();
            stream.Close();
            composeImage = null;
            drawingVisual = null;
            bgImage = null;
            encoder = null;
            stream = null;
        }
    }
}
