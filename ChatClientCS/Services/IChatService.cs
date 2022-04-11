using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChatClientCS.Enums;
using ChartProtocol;

namespace ChatClientCS.Services
{
    /// <summary>
    /// 双向通知 聊天服务
    /// </summary>
    public interface IChatService
    {
        #region 服务端推送
        /// <summary>
        /// 服务端通知用户登录
        /// </summary>
        event Action<User> ParticipantLoggedIn;
        /// <summary>
        /// 服务端通知用户推出
        /// </summary>
        event Action<string> ParticipantLoggedOut;
        /// <summary>
        /// 服务端通知用户断开连接
        /// </summary>
        event Action<string> ParticipantDisconnected;
        /// <summary>
        /// 服务端通知用户重新连接
        /// </summary>
        event Action<string> ParticipantReconnected;
        /// <summary>
        /// SignalR建立连接中
        /// </summary>
        event Action ConnectionReconnecting;
        /// <summary>
        /// SignalR已连接 (上面的事件,是不是有类似的了,重复了?)
        /// </summary>
        event Action ConnectionReconnected;
        /// <summary>
        /// SignalR已关闭
        /// </summary>
        event Action ConnectionClosed;
        /// <summary>
        /// 服务端文本消息, 广播及单播 推送
        /// </summary>
        event Action<string, string, MessageType> NewTextMessageReceived;
        /// <summary>
        /// 服务端图片消息, 广播及单播 推送
        /// </summary>
        event Action<string, byte[], MessageType> NewImageMessageReceived;
        /// <summary>
        /// 服务端通知正在打字
        /// </summary>
        event Action<string> ParticipantTyping;
        #endregion

        #region 客户端请求
        /// <summary>
        /// 建立SignalR链接
        /// </summary>
        /// <returns></returns>
        Task ConnectAsync();
        /// <summary>
        /// 本地登录, 调用服务端登录
        /// </summary>
        /// <param name="name">用户名</param>
        /// <param name="photo">头像</param>
        /// <returns>当前登录的其他用户</returns>
        Task<List<User>> LoginAsync(string name, byte[] photo);
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        Task LogoutAsync();
        /// <summary>
        /// 广播文本
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        Task SendBroadcastMessageAsync(string msg);
        /// <summary>
        /// 广播图片
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        Task SendBroadcastMessageAsync(byte[] img);
        /// <summary>
        /// 单播文本
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        Task SendUnicastMessageAsync(string receiver, string msg);
        /// <summary>
        /// 单播图片
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="img"></param>
        /// <returns></returns>
        Task SendUnicastMessageAsync(string receiver, byte[] img);
        /// <summary>
        /// 请求发给服务端, 告知本人正在打字
        /// </summary>
        /// <param name="receiver">接收者</param>
        /// <returns></returns>
        Task TypingAsync(string receiver);
        #endregion
    }
}