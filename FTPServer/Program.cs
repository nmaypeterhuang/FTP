using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mike;

namespace FTPServer
{
    class Program
    {
        public static Log logger = new Log();

        static void Main(string[] args)
        {
            //設定LOG物件
            string logName = "FTPServer." + DateTime.Now.yyyyMMdd() + "." + DateTime.Now.HHmmss();
            logger.SetLogObject(logName);
            logger.Info("開始執行");

            try
            {
                MainProcess.ProgramProcess();
            }
            catch (Exception ex)
            {
                logger.ErrorException("未預期錯誤", ex);
            }
        }
    }
}
