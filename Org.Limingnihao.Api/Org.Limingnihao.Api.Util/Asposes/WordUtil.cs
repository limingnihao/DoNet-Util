using Aspose.Words;
using Aspose.Words.Saving;
using Common.Logging;
using Org.Limingnihao.Api.Util;
using System;
using System.IO;

namespace Org.Limingnihao.Api.Asposes
{
    /// <summary>
    /// 使用Aspose的关于Word的相关操作
    /// </summary>
    public class WordUtil
    {
        private static readonly ILog logger = LogManager.GetLogger("WordUtil");

        /// <summary>
        /// Word转为图片
        /// </summary>
        /// <param name="source">word文件路径</param>
        /// <param name="target">图片保存的文件夹路径</param>
        /// <param name="resolution">分辨率</param>
        /// <param name="format">图片格式</param>
        public static bool ConverToImage(string source, string target, int resolution = 300, AsposeConvertDelegate d=null)
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
            LoadOptions loadOptions = new LoadOptions();
            loadOptions.LoadFormat = LoadFormat.Auto;
            Document doc = new Document(source, loadOptions);
            total = doc.PageCount;
            if (d != null)
            {
                second = (DateTime.Now - startTime).TotalSeconds;
                percent = 0.2;
                message = "开始转换文件，共" + total + "页！";
                d.Invoke(percent, page, total, second, path, message);
            }
            logger.Info("ConverToImage - source=" + source + ", target=" + target + ", resolution=" + resolution + ", pageCount=" + total);
            for (page = 0; page < total; page++)
            {
                ImageSaveOptions options = new ImageSaveOptions(SaveFormat.Png);
                options.PrettyFormat = false;
                options.Resolution = resolution;
                options.PageIndex = page;
                options.PageCount = 1;
                path = target + "\\" + (page + 1) + ".png";
                doc.Save(path, options);
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
        /// ppt转为pdf
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
            LoadOptions loadOptions = new LoadOptions();
            loadOptions.LoadFormat = LoadFormat.Auto;
            Document doc = new Document(source, loadOptions);
            int pageCount = doc.PageCount;
            logger.Info("ConverToPdf - source=" + source + ", target=" + target + ", pageCount=" + pageCount);
            PdfSaveOptions options = new PdfSaveOptions();
            doc.Save(target, options);
            return true;
        }
    
    }
}
