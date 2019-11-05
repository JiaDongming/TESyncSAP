using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using log4net;

namespace SAPSyncdll
{
    /// <summary>
    /// 日志类
    /// </summary>
    public class LogHelper
    {

        //日志目录
        //static string filePath = @"D:\MyServiceLog.txt";
        static string filePath = ConfigurationManager.AppSettings["filePath"];
        public static void WriteLog(string message)
        {
            ILog log = GetLog();
            if (log != null)
            {
                log.Info(message);

            }
            return;
            using (FileStream stream = new FileStream(filePath, FileMode.Append))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.WriteLine($"{DateTime.Now}," + message);
            }
        }
        static public ILog GetLog()
        {
            return LogManager.GetLogger("Main");
        }

        static public void Error(string message, Exception ex)
        {
            ILog log = GetLog();
            if (log != null)
            {
                log.Error(message, ex);
            }
        }
    }
}
