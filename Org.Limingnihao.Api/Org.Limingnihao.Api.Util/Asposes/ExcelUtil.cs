using Aspose.Cells;
using Aspose.Cells.Rendering;
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
    /// 使用Aspose对Excel相关操作，转图片，转pdf等
    /// </summary>
    public class ExcelUtil
    {
        private static readonly ILog logger = LogManager.GetLogger("ExcelUtil");

        /// <summary>
        /// Excel转为图片
        /// </summary>
        /// <param name="source">源文件路径</param>
        /// <param name="target">图片保存的文件夹路径</param>
        /// <param name="dpi">dpi</param>
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

            LoadOptions loadOptions = new LoadOptions(LoadFormat.Auto);
            Workbook workbook = new Workbook(source, loadOptions);
            total = workbook.Worksheets.Count;

            if (d != null)
            {
                second = (DateTime.Now - startTime).TotalSeconds;
                percent = 0.2;
                message = "开始转换文件，共" + total + "页！";
                d.Invoke(percent, page, total, second, path, message);
            }
            logger.Info("ConverToImage - source=" + source + ", target=" + target + ", pageCount=" + total);
            for (page = 0; page < total; page++)
            {
                Worksheet sheet = workbook.Worksheets[page];
                ImageOrPrintOptions op = new ImageOrPrintOptions();
                op.ImageFormat = ImageFormat.Png;
                op.HorizontalResolution = resolution;
                op.VerticalResolution = resolution;
                SheetRender sr = new SheetRender(sheet, op);
                for (int j = 0; j < sr.PageCount; j++ )
                {
                    Bitmap bitmap = sr.ToImage(j);
                    path = target + "\\" + (page + 1) + "_" + (j + 1) + ".png";
                    bitmap.Save(path);
                    if (d != null)
                    {
                        second = (DateTime.Now - startTime).TotalSeconds;
                        percent = 0.2 + (page + 1) * 0.8 / total;
                        message = "正在转换第" + (page + 1) + "/" + total + "页！";
                        d.Invoke(percent, (page + 1), total, second, path, message);
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Excel转为pdf
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
            LoadOptions loadOptions = new LoadOptions(LoadFormat.Auto);
            Workbook workbook = new Workbook(source, loadOptions);
            int pageCount = workbook.Worksheets.Count;
            logger.Info("ConverToPdf - source=" + source + ", target=" + target + ", pageCount=" + pageCount);
            workbook.Save(target, SaveFormat.Pdf);
            return true;
        }
    }
}
