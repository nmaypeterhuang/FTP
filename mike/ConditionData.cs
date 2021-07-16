using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChiefPayBatch
{
    public class ConditionData
    {
        /// <summary>
        /// 建構式
        /// </summary>
        public ConditionData()
        {
        }

        /// <summary>
        /// 起始日期
        /// </summary>
        public string StartDate = "";
        /// <summary>
        /// 結束日期
        /// </summary>
        public string CloseDate = "";
        /// <summary>
        /// 起始月
        /// </summary>
        public string StartMonth = "";
        /// <summary>
        /// 結束月
        /// </summary>
        public string CloseMonth = "";

        /// <summary>
        /// 業者
        /// </summary>
        public string MerchantID = "";
        /// <summary>
        /// 門店
        /// </summary>
        public string CounterID = "";
        /// <summary>
        /// 是否為重跑
        /// </summary>
        public bool IsReRun = false;
        /// <summary>
        /// 是否要清除資料
        /// </summary>
        public bool IsReset = false;
        /// <summary>
        /// 是否有在指令中指定日期
        /// </summary>
        public bool AssignDate = false;
        /// <summary>
        /// 是否為UPLOAD
        /// </summary>
        public string FTPType = "B";


        /// <summary>
        /// 是否先跑下載
        /// </summary>
        public bool RunDownload = true;

        /// <summary>
        /// 起始時間
        /// </summary>
        public string StartTime = "";
        /// <summary>
        /// 結束時間
        /// </summary>
        public string CloseTime = "";
        /// <summary>
        /// 要執行的動作
        /// </summary>
        public ActionCode Action;
        /// <summary>
        /// FILE_INFO.FILE_NAME
        /// </summary>
        public string FileNameStart;
        /// <summary>
        /// 檔案路徑
        /// </summary>
        public string FilePath;
        /// <summary>
        /// 是否有在指令中指定檔案名稱
        /// </summary>
        public bool AssignFileName = false;


        /// <summary>
        /// 檢查資料異常
        /// </summary>
        /// <returns></returns>
        public string CheckDataError()
        {
            string errMsg = "";
            if (!string.IsNullOrEmpty(StartDate))
            {
                try
                {
                    DateTime.ParseExact(StartDate, "yyyyMMdd", null);
                }
                catch (Exception ex)
                {
                    errMsg += "起始日期有誤" + StartDate + Environment.NewLine;
                }


            }
            if (!string.IsNullOrEmpty(CloseDate))
            {
                try
                {
                    DateTime.ParseExact(CloseDate, "yyyyMMdd", null);
                }
                catch (Exception ex)
                {
                    errMsg += "結束日期有誤" + CloseDate + Environment.NewLine;
                }
            }

            if (!string.IsNullOrEmpty(StartMonth))
            {
                try
                {
                    DateTime.ParseExact(StartMonth, "yyyyMM", null);
                }
                catch (Exception ex)
                {
                    errMsg += "起始月份有誤" + StartMonth + Environment.NewLine;
                }


            }

            if (!string.IsNullOrEmpty(CloseMonth))
            {
                try
                {
                    DateTime.ParseExact(CloseMonth, "yyyyMM", null);
                }
                catch (Exception ex)
                {
                    errMsg += "結束月份有誤" + CloseMonth + Environment.NewLine;
                }


            }


            return errMsg;
        }

        /// <summary>
        /// 執行動作列舉
        /// 新增項目一定要放在最下面以防INDEX錯亂
        /// </summary>
        public enum ActionCode
        {
            
            /// <summary>
            /// 悠遊卡帳務檔TRNC轉入
            /// </summary>
            IMPORT_TRNC,
            /// <summary>
            /// LINEPAY帳務檔轉入
            /// </summary>
            IMPORT_LINEPAY,
            /// <summary>
            /// 街口帳務檔轉入
            /// </summary>
            IMPORT_JKOPAY,
            /// <summary>
            /// 悠遊付帳務檔轉入
            /// </summary>
            IMPORT_EASYWALLET,
            /// <summary>
            /// 一卡通帳務檔轉入
            /// </summary>
            IMPORT_IPASSPAY,
            /// <summary>
            /// 未指定
            /// </summary>
            NONE
        }
    }
}
