using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Threading;

namespace mike
{
    public class BatchProcess
    {
        ConditionData oCondition;        

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="oCon"></param>
        public BatchProcess(ConditionData oCon)
        {
            oCondition = oCon;
        }

        /// <summary>
        /// 執行批次
        /// </summary>
        public void RunBatch()
        {
            if (oCondition == null)
            {
                return;
            }
            switch (oCondition.Action)
            {
                case ConditionData.ActionCode.FTP_SERVER:
                    FTPServer oFTPServer = new FTPServer();
                    oFTPServer.MainProcess();
                    break;
                case ConditionData.ActionCode.SFTP_SERVER:
                    SFTPServer oSFTPServer = new SFTPServer();
                    oSFTPServer.MainProcess();
                    break;
                case ConditionData.ActionCode.NONE:
                    break;
                default:
                    break;
            }
        }

    }
}
