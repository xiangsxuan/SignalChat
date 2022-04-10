namespace ChartProtocol
{
    public class User
    {
        /// <summary>
        /// 用戶名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 用戶Id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 用戶头像
        /// </summary>
        public byte[] Photo { get; set; }
    }
}