using Aspose.Note;
using Aspose.Note.Saving;
using Common.Logging;
using Org.Limingnihao.Api.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Org.Limingnihao.Api.Asposes
{
    /// <summary>
    /// 使用Aspose的关于OneNote的相关操作
    /// </summary>
    public class OneNoteUtil
    {
        private static readonly ILog logger = LogManager.GetLogger("OneNoteUtil");

        /// <summary>
        /// OneNote转为图片 - 未开发完
        /// </summary>
        /// <param name="source">源文件路径</param>
        /// <param name="target">图片保存的文件夹路径</param>
        /// <param name="dpi">dpi</param>
        /// <param name="format">图片格式</param>
        public static bool ConverToImage(string source, string target, float scale, AsposeConvertDelegate d = null)
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
            Document doc = new Document(source);
            if (d != null)
            {
                second = (DateTime.Now - startTime).TotalSeconds;
                percent = 0.2;
                message = "开始转换文件，共" + total + "页！";
                d.Invoke(percent, (page + 1), total, second, path, message);
            }
            logger.Info("ConverToImage - source=" + source + ", target=" + target + ", scale=" + scale + ", pageCount=" + total);
            ImageSaveOptions opts = new ImageSaveOptions(SaveFormat.Png);
            opts.PageIndex = 1;
            doc.Save(target + "\\" + (page + 1) + "_.png", opts);

            //for (page = 0; page < total; page++)
            //{
            //    if (d != null)
            //    {
            //        second = (DateTime.Now - startTime).TotalSeconds;
            //        percent = 0.2 + page * 0.8 / total;
            //        message = "正在转换第" + (page + 1) + "/" + total + "页！";
            //        d.Invoke(percent, page, total, second, message);
            //    }
            //}

            return true;
        }

        /// <summary>
        /// OneNote转为pdf
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
            Document doc = new Document(source);
            PdfSaveOptions opts = new PdfSaveOptions();
            doc.Save(target, opts);
            logger.Info("ConverToPdf - source=" + source + ", target=" + target + ", PageIndex=" + opts.PageIndex + ", PageCount=" + opts.PageCount);
            return true;
        }

    }
}
