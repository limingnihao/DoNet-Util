using Common.Logging;
using Org.Limingnihao.Api.Util;
using System.IO;

namespace Org.Limingnihao.Api.Asposes
{
    /// <summary>
    /// 转换文件的回调代理
    /// </summary>
    /// <param name="percent"></param>
    public delegate void AsposeConvertDelegate(double percent, int page, int total, double second, string path, string message);

    public class AsposeUtil
    {
        private static readonly ILog logger = LogManager.GetLogger("AsposeUtil");

        /// <summary>
        /// 转换文件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool Convert(string source, string target, AsposeConvertDelegate d)
        {
            logger.Info("Convert - source=" + source + ", type=" + FileTypeUtil.GetFileType(source));
            if (FileTypeUtil.IsWordFile(source))
            {
                return WordUtil.ConverToImage(source, target, 300, d);
            }
            if (FileTypeUtil.IsExcelFile(source))
            {
                return ExcelUtil.ConverToImage(source, target, 300, d);
            }
            if (FileTypeUtil.IsPowerPointFile(source))
            {
                return PowerpointUtil.ConverToImage(source, target, 1.5F, d);
            }
            if (FileTypeUtil.IsVisioFile(source))
            {
                return VisioUtil.ConverToImage(source, target, 100, d);
            }
            if(FileTypeUtil.IsPdfFile(source)){
                return PDFUtil.ConverToImage(source, target, 100, d);
            }
            throw new IOException("文件格式当前不支持！");
        }
    }
}
