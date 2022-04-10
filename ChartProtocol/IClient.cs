namespace ChartProtocol
{
    // 客户端可以调用的方法, 接口定义。利于服务端强引用实现
    public interface IClient
    {
        /// <summary>
        /// 参会者断开连接
        /// </summary>
        /// <param name="name"></param>
        void ParticipantDisconnection(string name);
        /// <summary>
        /// 参会者重新连接
        /// </summary>
        /// <param name="name"></param>
        void ParticipantReconnection(string name);
        void ParticipantLogin(User client);
        void ParticipantLogout(string name);
        void BroadcastTextMessage(string sender, string message);
        void BroadcastPictureMessage(string sender, byte[] img);
        void UnicastTextMessage(string sender, string message);
        void UnicastPictureMessage(string sender, byte[] img);
        void ParticipantTyping(string sender);
    }
}