using System;
using System.IO;
using System.Text;
using System.Net;
using System.Collections.Generic;

namespace mike
{
    public class FTPServer
    {
        public FTPServer()
        {
            logger.Info(logger.WriteStartLog(ProgName));
        }

        string ProgName = "FTPServer";
        Log logger = new Log();

        /// <summary>
        /// 主流程
        /// </summary>
        public void MainProcess()
        {
            string filename = @"a.txt";

            UploadFile(filename);

            List<string> fileList = GetFileList();

            foreach (string file in fileList)
            {
                DownloadFile(file);
            }

            logger.Info(logger.WriteEndLog(ProgName));
        }

        /// <summary>
        /// ftp下載檔案
        /// </summary>
        /// <param name="fileName">僅檔名，不含路徑</param>
        private void DownloadFile(string fileName)
        {
            logger.Info("開始: FTP下載檔案");

            string ftpUrl = System.Configuration.ConfigurationManager.AppSettings["FTP_URL"];
            string ftpFilePath = System.Configuration.ConfigurationManager.AppSettings["FTP_EXPORT_PATH"]._GetUrl();
            string ftpPort = System.Configuration.ConfigurationManager.AppSettings["FTP_PORT"];
            string ftpID = System.Configuration.ConfigurationManager.AppSettings["FTP_ID"];
            string ftpPass = System.Configuration.ConfigurationManager.AppSettings["FTP_PASS"];

            int Port = 0;
            try
            {
                Port = Convert.ToInt32(ftpPort);
            }
            catch (Exception)
            {
            }

            string filePath = System.Configuration.ConfigurationManager.AppSettings["IMPORT_PATH"]._GetPath();

            if (Directory.Exists(filePath) == false)
            {
                Directory.CreateDirectory(filePath);
            }

            string remoteFilePath = ftpUrl + ftpFilePath + fileName;
            string localFilePath = filePath + fileName;

            FtpWebRequest reqFTP;

            // 緩衝大小設定為2kb
            int bufferSize = 2048;

            byte[] buffer = new byte[bufferSize];
            long contentLen;
            int readCount;

            try
            {
                logger.Info("開始: FTP連線");
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(remoteFilePath));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpID, ftpPass);
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;

                if (File.Exists(localFilePath))
                {
                    File.Delete(localFilePath);
                }

                using (FileStream fs = new FileStream(localFilePath, FileMode.Create))
                {
                    using (FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse())
                    {
                        bool fileExists = GetFileList().Exists(x => x == fileName);

                        if (fileExists == false)
                        {
                            logger.Info("伺服器上檔案" + fileName + "不存在");
                        }
                        else
                        {
                            contentLen = response.ContentLength;

                            logger.Info("開始: 下載檔案: " + fileName);
                            using (Stream ftpStream = response.GetResponseStream())
                            {
                                readCount = ftpStream.Read(buffer, 0, bufferSize);

                                while (readCount > 0)
                                {
                                    fs.Write(buffer, 0, readCount);

                                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                                }
                            }
                            logger.Info("結束: 下載檔案: " + fileName + "完成");
                        }
                    }
                }
                logger.Info("結束: FTP連線完成");
                reqFTP.Abort();
            }
            catch (Exception ex)
            {
                logger.ErrorException("結束: FTP下載失敗", ex);
            }
        }

        /// <summary>
        /// ftp上傳檔案
        /// </summary>
        /// <param name="filename">僅檔名，不含路徑</param>
        private void UploadFile(string fileName)
        {
            logger.Info("開始: FTP上傳檔案");

            string ftpUrl = System.Configuration.ConfigurationManager.AppSettings["FTP_URL"];
            string ftpFilePath = System.Configuration.ConfigurationManager.AppSettings["FTP_IMPORT_PATH"]._GetUrl();
            string ftpPort = System.Configuration.ConfigurationManager.AppSettings["FTP_PORT"];
            string ftpID = System.Configuration.ConfigurationManager.AppSettings["FTP_ID"];
            string ftpPass = System.Configuration.ConfigurationManager.AppSettings["FTP_PASS"];

            int Port = 0;
            try
            {
                Port = Convert.ToInt32(ftpPort);
            }
            catch (Exception)
            {
            }

            string filePath = System.Configuration.ConfigurationManager.AppSettings["EXPORT_PATH"]._GetPath();

            string remoteFilePath = ftpUrl + ftpFilePath + fileName;
            string localFilePath = filePath + fileName;

            FileInfo fileInf = new FileInfo(localFilePath);
            FtpWebRequest reqFTP;

            // 緩衝大小設定為2kb
            int bufferSize = 2048;

            byte[] buffer = new byte[bufferSize];
            int contentLen;

            if (File.Exists(filePath + fileName) == false)
            {
                logger.Info("本機檔案" + fileName + "不存在");
            }
            else
            {
                try
                {
                    logger.Info("開始: FTP連線");
                    reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(remoteFilePath));
                    reqFTP.UseBinary = true;
                    reqFTP.KeepAlive = false;
                    reqFTP.Credentials = new NetworkCredential(ftpID, ftpPass);
                    reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                    reqFTP.ContentLength = fileInf.Length;

                    using (FileStream fs = fileInf.OpenRead())
                    {
                        logger.Info("開始: 上傳檔案: " + fileName);
                        using (Stream ftpStream = reqFTP.GetRequestStream())
                        {
                            contentLen = fs.Read(buffer, 0, bufferSize);

                            while (contentLen != 0)
                            {
                                // 把內容從file stream 寫入 upload stream
                                ftpStream.Write(buffer, 0, bufferSize);

                                contentLen = fs.Read(buffer, 0, bufferSize);
                            }
                        }
                        logger.Info("結束: 上傳檔案: " + fileName + "完成");
                    }
                    logger.Info("結束: FTP連線完成");
                    reqFTP.Abort();
                }
                catch (Exception ex)
                {
                    logger.ErrorException("結束: FTP上傳失敗", ex);
                }
            }
        }

        /// <summary>
        /// ftp獲得檔案列表
        /// </summary>
        /// <returns>檔案列表</returns>
        public List<string> GetFileList()
        {
            logger.Info("開始: FTP獲得檔案列表");

            string ftpUrl = System.Configuration.ConfigurationManager.AppSettings["FTP_URL"];
            string ftpFilePath = System.Configuration.ConfigurationManager.AppSettings["FTP_EXPORT_PATH"]._GetUrl();
            string ftpPort = System.Configuration.ConfigurationManager.AppSettings["FTP_PORT"];
            string ftpID = System.Configuration.ConfigurationManager.AppSettings["FTP_ID"];
            string ftpPass = System.Configuration.ConfigurationManager.AppSettings["FTP_PASS"];

            int Port = 0;
            try
            {
                Port = Convert.ToInt32(ftpPort);
            }
            catch (Exception)
            {
            }

            List<string> fileList = new List<string>();
            FtpWebRequest reqFTP;

            try
            {
                logger.Info("開始: FTP連線");
                reqFTP = (FtpWebRequest)WebRequest.Create(new Uri(ftpUrl + ftpFilePath));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpID, ftpPass);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;

                using (WebResponse response = reqFTP.GetResponse())
                {
                    logger.Info("開始: 獲得目錄");
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default))
                    {
                        string line = reader.ReadLine();
                        while (line != null)
                        {
                            fileList.Add(line);
                            line = reader.ReadLine();
                        }
                    }
                    logger.Info("結束: 獲得目錄完成");
                }
                reqFTP.Abort();
                logger.Info("結束: FTP連線完成");
            }
            catch (Exception ex)
            {
                logger.ErrorException("結束: FTP獲得失敗", ex);
            }

            return fileList;
        }
    }
}