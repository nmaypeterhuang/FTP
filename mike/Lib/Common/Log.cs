using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mike
{
    public class Log
    {
        static string log_path = System.Configuration.ConfigurationManager.AppSettings["LOG_PATH"]._GetPath();
        static string log_filename;
        
        public void SetLogObject(string logFileName)
        {
            log_filename = logFileName + ".txt";

            if (!Directory.Exists(log_path))
            {
                Directory.CreateDirectory(log_path);
            }

            //Create會鎖住檔案
            //StreamWriter會自動判斷檔案是否存在，不存在則會創建
            //if (!File.Exists(log_path))
            //{
            //    File.Create(log_path);
            //}
        }

        public void Info(string logMsg)
        {
            using (StreamWriter sw = File.AppendText(log_path + log_filename))
            {
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffff") + " [INFO] > " + logMsg);
            }
        }

        public string WriteStartLog(string progName)
        {
            return progName + "開始執行" + Environment.NewLine;
        }

        public string WriteEndLog(string progName)
        {
            return progName + "執行完成" + Environment.NewLine;
        }

        public void ErrorException(string logMsg, Exception ex)
        {
            using (StreamWriter sw = File.AppendText(log_path + log_filename))
            {
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffff") + " [Error] > " + logMsg + ex.ToString());
            }
        }
    }
}
