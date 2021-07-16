namespace mike
{
    public class ConditionData
    {
        /// <summary>
        /// 要執行的動作
        /// </summary>
        public ActionCode Action;

        /// <summary>
        /// 執行動作列舉
        /// 新增項目一定要放在最下面以防INDEX錯亂
        /// </summary>
        public enum ActionCode
        {
            /// <summary>
            /// ftp
            /// </summary>
            FTP_SERVER,
            /// <summary>
            /// sftp
            /// </summary>
            SFTP_SERVER,
            /// <summary>
            /// 未指定
            /// </summary>
            NONE
        }
    }
}
