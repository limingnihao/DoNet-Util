using Aspose.Pdf;
using Aspose.Pdf.Devices;
using Aspose.Pdf.Facades;
using Common.Logging;
using Org.Limingnihao.Api.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace Org.Limingnihao.Api.Asposes
{
    /// <summary>
    /// 使用Aspose的关于PDF的相关操作
    /// </summary>
    public class PDFUtil
    {
        private static readonly ILog logger = LogManager.GetLogger("PDFUtil");

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
            Document document = new Document(source);
            total = document.Pages.Count;
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
                path = target + "\\" + (page + 1) + ".png";
                using (FileStream imageStream = new FileStream(path, FileMode.Create))
                {
                    PngDevice pngDevice = new PngDevice(new Resolution(resolution));
                    pngDevice.Process(document.Pages[page+1], imageStream);
                    imageStream.Close();
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


        public static bool ConverToPdf(string source, string target, string fileType, AsposeConvertDelegate d = null)
        {
            if (!Directory.Exists(source))
            {
                throw new DirectoryNotFoundException();
            }

            DirectoryInfo di = new DirectoryInfo(source);
            List<String> files = new List<string>();
            foreach (FileInfo f in di.GetFiles())
            {
                if (FileUtil.GetFileType(f.FullName).Equals(fileType))
                {
                    files.Add(f.FullName);
                }
            }
            if (files.Count != 0)
            {
                return ConverToPdf(files, target, d);
            }
            else
            {
                return false;
            }
        }

        public static bool ConverToPdf(List<String> files, string target, AsposeConvertDelegate d = null)
        {
            logger.Info("ConverToPdf - files=" + files.Count + " , target=" + target );
            double percent = 0.0;
            int page = 0;
            int total = 0;
            double second = 0;
            string path = "";
            string message = "";
            DateTime startTime = DateTime.Now;

            if (File.Exists(target))
            {
                FileInfo fi = new FileInfo(target);
                fi.Delete();
                logger.Debug("ConverToPdf - delete - fileName=" + fi.FullName);
            }
            Document document = new Document();
            total = files.Count;
            if (d != null)
            {
                second = (DateTime.Now - startTime).TotalSeconds;
                percent = 0.2;
                message = "开始添加文件，共" + total + "页！";
                d.Invoke(percent, page, total, second, path, message);
            }

            for (page = 0; page < total; page++)
            {
                path = files[page];
                Page pdfPage = document.Pages.Insert(page + 1);
                BitmapImage bitemap = new BitmapImage();
                bitemap.BeginInit();
                bitemap.StreamSource = new FileStream(path, FileMode.Open);
                bitemap.EndInit();
                logger.Debug("ConverToPdf - PixelWidth=" + bitemap.PixelWidth + ", PixelHeight=" + bitemap.PixelHeight + ", path=" + path);
                Aspose.Pdf.Rectangle rec = new Aspose.Pdf.Rectangle(0, 0, bitemap.PixelWidth, bitemap.PixelHeight);
                pdfPage.SetPageSize(bitemap.PixelWidth, bitemap.PixelHeight);
                pdfPage.AddImage(bitemap.StreamSource, rec, bitemap.PixelWidth, bitemap.PixelHeight, true);

                if (d != null)
                {
                    second = (DateTime.Now - startTime).TotalSeconds;
                    percent = 0.2 + (page + 1) * 0.8 / total;
                    message = "正在添加第" + (page + 1) + "/" + total + "个文件！";
                    d.Invoke(percent, (page + 1), total, second, path, message);
                }
            }
            document.Save(target);
            return true;
        }

    }
}
