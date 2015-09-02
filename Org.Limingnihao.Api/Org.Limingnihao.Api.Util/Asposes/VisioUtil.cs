using Aspose.Diagram;
using Aspose.Diagram.Saving;
using Common.Logging;
using Org.Limingnihao.Api.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace Org.Limingnihao.Api.Asposes
{
    /// <summary>
    /// 使用Aspose的关于visio的相关操作
    /// </summary>
    public class VisioUtil
    {
        private static readonly ILog logger = LogManager.GetLogger("WordUtil");

        /// <summary>
        /// Word转为图片
        /// </summary>
        /// <param name="source">word文件路径</param>
        /// <param name="target">图片保存的文件夹路径</param>
        /// <param name="resolution">分辨率</param>
        /// <param name="format">图片格式</param>
        public static bool ConverToImage(string source, string target, int resolution = 300, AsposeConvertDelegate d = null)
        {
            double percent = 0.0;
            int page = 0;
            int total = 0;
            double second = 0;
            string path = "";
            string message = "";
            DateTime startTime = DateTime.Now;
            if (!FileUtil.CreateDirectory(target))
            {
                throw new DirectoryNotFoundException();
            }
            if (!File.Exists(source))
            {
                throw new FileNotFoundException();
            }
            if (d != null)
            {
                second = (DateTime.Now - startTime).TotalSeconds;
                percent = 0.1;
                message = "正在解析文件！";
                d.Invoke(percent, page, total, second, path, message);
            }
            Diagram diagram = new Diagram(source);
            total = diagram.Pages.Count;
            if (d != null)
            {
                second = (DateTime.Now - startTime).TotalSeconds;
                percent = 0.2;
                message = "开始转换文件，共" + total + "页！";
                d.Invoke(percent, page, total, second, path, message);
            }
            logger.Info("ConverToImage - source=" + source + ", target=" + target + ", resolution=" + resolution + ", pageCount=" + total);
            ImageSaveOptions options = new ImageSaveOptions(SaveFileFormat.PNG);
            options.Resolution = resolution;

            for (page = 0; page < total; page++)
            {
                options.PageIndex = page;
                using (MemoryStream stream = new MemoryStream())
                {
                    diagram.Save(stream, options);
                    Bitmap bitmap = new Bitmap(stream);
                    path = target + "\\" + (page + 1) + "_.png";
                    bitmap.Save(path, ImageFormat.Png);
                }
                if (d != null)
                {
                    second = (DateTime.Now - startTime).TotalSeconds;
                    percent = 0.2 + (page + 1) * 0.8 / total;
                    message = "正在转换第" + (page + 1) + "/" + total + "页！";
                    d.Invoke(percent, (page + 1), total, second, path, message);
                }
            }
            return true;
        }


        /// <summary>
        /// visio转为pdf
        /// </summary>
        /// <param name="source">源文件路径</param>
        /// <param name="target">目标文件路径</param>
        /// <returns></returns>
        public static bool ConverToPdf(string source, string target)
        {
            FileUtil.DeleteFile(target);
            if (!File.Exists(source))
            {
                throw new FileNotFoundException();
            }
            Diagram diagram = new Diagram(source);
            int pageCount = diagram.Pages.Count;
            logger.Info("ConverToPdf - source=" + source + ", target=" + target + ", pageCount=" + pageCount);
            MemoryStream pdfStream = new MemoryStream();
            diagram.Save(pdfStream, SaveFileFormat.PDF);
            FileStream stream = new FileStream(target, FileMode.CreateNew, FileAccess.Write);
            stream.Write(pdfStream.GetBuffer(), 0, (int)pdfStream.Length);
            return true;
        }
    
    }
}
