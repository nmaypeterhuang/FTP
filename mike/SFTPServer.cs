using System;
using System.IO;
using System.Collections.Generic;
using Renci.SshNet;

namespace mike
{
    public class SFTPServer
    {
        public SFTPServer()
        {
            logger.Info(logger.WriteStartLog(ProgName));
        }

        string ProgName = "SFTPServer";
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
        /// sftp下載檔案
        /// </summary>
        /// <param name="fileName">僅檔名，不含路徑</param>
        private void DownloadFile(string fileName)
        {
            logger.Info("開始: SFTP下載檔案");

            string ftpUrl = System.Configuration.ConfigurationManager.AppSettings["FTP_URL"];
            string ftpFilePath = System.Configuration.ConfigurationManager.AppSettings["FTP_EXPORT_PATH"]._GetUrl();
            string ftpPort = System.Configuration.ConfigurationManager.AppSettings["FTP_PORT"];
            string ftpID = System.Configuration.ConfigurationManager.AppSettings["FTP_ID"];
            string ftpPass = System.Configuration.ConfigurationManager.AppSettings["FTP_PASS"];

            int Port = 22;
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

            try
            {
                logger.Info("開始: SFTP連線");
                List<AuthenticationMethod> authMethods = new List<AuthenticationMethod>();
                authMethods.Add(new PasswordAuthenticationMethod(ftpID, ftpPass));

                ConnectionInfo sshConnectionInfo = new ConnectionInfo(ftpUrl, Port, ftpID, authMethods.ToArray());

                if (File.Exists(localFilePath))
                {
                    File.Delete(localFilePath);
                }

                using (SftpClient client = new SftpClient(sshConnectionInfo))
                {
                    client.Connect();
                    bool fileExists = GetFileList().Exists(x => x == fileName);

                    if (fileExists == false)
                    {
                        logger.Info("伺服器上檔案" + fileName + "不存在");
                    }
                    else
                    {
                        logger.Info("開始: 下載檔案: " + fileName);
                        using (Stream file = File.OpenWrite(localFilePath))
                        {
                            client.DownloadFile(remoteFilePath, file);
                        }
                        logger.Info("結束: 下載檔案: " + fileName + "完成");
                    }

                    client.Disconnect();
                }
                logger.Info("結束: SFTP連線完成");
            }
            catch (Exception ex)
            {
                logger.ErrorException("結束: SFTP下載失敗", ex);
            }
        }

        /// <summary>
        /// ftp上傳檔案
        /// </summary>
        /// <param name="filename">僅檔名，不含路徑</param>
        private void UploadFile(string fileName)
        {
            logger.Info("開始: SFTP上傳檔案");

            string ftpUrl = System.Configuration.ConfigurationManager.AppSettings["FTP_URL"];
            string ftpFilePath = System.Configuration.ConfigurationManager.AppSettings["FTP_IMPORT_PATH"]._GetUrl();
            string ftpPort = System.Configuration.ConfigurationManager.AppSettings["FTP_PORT"];
            string ftpID = System.Configuration.ConfigurationManager.AppSettings["FTP_ID"];
            string ftpPass = System.Configuration.ConfigurationManager.AppSettings["FTP_PASS"];

            int Port = 22;
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

            if (File.Exists(filePath + fileName) == false)
            {
                logger.Info("本機檔案" + fileName + "不存在");
            }
            else
            {
                try
                {
                    logger.Info("開始: SFTP連線");
                    List<AuthenticationMethod> authMethods = new List<AuthenticationMethod>();
                    authMethods.Add(new PasswordAuthenticationMethod(ftpID, ftpPass));

                    ConnectionInfo sshConnectionInfo = new ConnectionInfo(ftpUrl, Port, ftpID, authMethods.ToArray());

                    using (SftpClient client = new SftpClient(sshConnectionInfo))
                    {
                        client.Connect();
                        logger.Info("開始: 上傳檔案: " + fileName);

                        using (var file = File.OpenRead(localFilePath))
                        {
                            client.UploadFile(file, remoteFilePath);
                        }

                        logger.Info("結束: 上傳檔案: " + fileName + "完成");
                        client.Disconnect();
                    }
                    logger.Info("結束: SFTP連線完成");
                }
                catch (Exception ex)
                {
                    logger.ErrorException("結束: SFTP上傳失敗", ex);
                }
            }
        }

        /// <summary>
        /// sftp獲得檔案列表
        /// </summary>
        /// <returns>檔案列表</returns>
        public List<string> GetFileList()
        {
            logger.Info("開始: SFTP獲得檔案列表");

            string ftpUrl = System.Configuration.ConfigurationManager.AppSettings["FTP_URL"];
            string ftpFilePath = System.Configuration.ConfigurationManager.AppSettings["FTP_EXPORT_PATH"]._GetUrl();
            string ftpPort = System.Configuration.ConfigurationManager.AppSettings["FTP_PORT"];
            string ftpID = System.Configuration.ConfigurationManager.AppSettings["FTP_ID"];
            string ftpPass = System.Configuration.ConfigurationManager.AppSettings["FTP_PASS"];

            int Port = 22;
            try
            {
                Port = Convert.ToInt32(ftpPort);
            }
            catch (Exception)
            {
            }

            string remoteFilePath = ftpUrl + ftpFilePath;

            List<string> fileList = new List<string>();

            try
            {
                logger.Info("開始: SFTP連線");
                List<AuthenticationMethod> authMethods = new List<AuthenticationMethod>();
                authMethods.Add(new PasswordAuthenticationMethod(ftpID, ftpPass));

                ConnectionInfo sshConnectionInfo = new ConnectionInfo(ftpUrl, Port, ftpID, authMethods.ToArray());

                using (SftpClient client = new SftpClient(sshConnectionInfo))
                {
                    client.Connect();
                    logger.Info("開始: 獲得目錄");
                    var files = client.ListDirectory(remoteFilePath);
                    logger.Info("結束: 獲得目錄完成");
                    client.Disconnect();
                }
                logger.Info("結束: SFTP連線完成");
            }
            catch (Exception ex)
            {
                logger.ErrorException("結束: SFTP獲得失敗", ex);
            }

            return fileList;
        }
    }
}