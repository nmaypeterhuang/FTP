using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mike;

namespace FTPServer
{
    class MainProcess
    {
        public static Log logger = new Log();

        /// <summary>
        /// 程式主流程
        /// </summary>
        public static void ProgramProcess()
        {
            ConditionData oCondition = new ConditionData();
            oCondition.Action = ConditionData.ActionCode.FTP_SERVER;

            // 開始執行
            BatchProcess oBatch = new BatchProcess(oCondition);
            oBatch.RunBatch();
        }
    }
}
