namespace ChartProtocol
{
    // 客户端可以调用的方法, 接口定义。利于服务端强引用实现
    public interface IClient
    {
        /// <summary>
        /// 参与者断开连接
        /// </summary>
        /// <param name="name"></param>
        void ParticipantDisconnection(string name);
        /// <summary>
        /// 参与者重新连接
        /// </summary>
        /// <param name="name"></param>
        void ParticipantReconnection(string name);
        /// <summary>
        /// 参与者登录
        /// </summary>
        /// <param name="client"></param>
        void ParticipantLogin(User client);
        /// <summary>
        /// 参与者退出
        /// </summary>
        /// <param name="name"></param>
        void ParticipantLogout(string name);
        /// <summary>
        /// 广播消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        void BroadcastTextMessage(string sender, string message);
        /// <summary>
        /// 广播图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="img"></param>
        void BroadcastPictureMessage(string sender, byte[] img);
        /// <summary>
        /// 文本私聊
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        void UnicastTextMessage(string sender, string message);
        /// <summary>
        /// 图片私聊
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="img"></param>
        void UnicastPictureMessage(string sender, byte[] img);
        /// <summary>
        /// 对面正在输入通知
        /// </summary>
        /// <param name="sender"></param>
        void ParticipantTyping(string sender);
    }
}