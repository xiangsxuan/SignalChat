using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Microsoft.AspNet.SignalR;
using ChartProtocol;

namespace ChatServerCS
{
    public class ChatHub : Hub<IClient>
    {
        /// <summary>
        /// 所有登录用户
        /// 用户名->用户全部信息
        /// </summary>
        private static ConcurrentDictionary<string, User> ChatClients = new ConcurrentDictionary<string, User>();

        public override Task OnDisconnected(bool stopCalled)
        {
            var userName = ChatClients.SingleOrDefault((c) => c.Value.ID == Context.ConnectionId).Key;
            if (userName != null)
            {
                // 通知其他客户端, 本客户端已下线
                Clients.Others.ParticipantDisconnection(userName);
                Console.WriteLine($"<> {userName} disconnected");
            }
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            var userName = ChatClients.SingleOrDefault((c) => c.Value.ID == Context.ConnectionId).Key;
            if (userName != null)
            {
                // 通知其他客户端, 本客户端又上线了
                Clients.Others.ParticipantReconnection(userName);
                Console.WriteLine($"== {userName} reconnected");
            }
            return base.OnReconnected();
        }

        public List<User> Login(string name, byte[] photo)
        {
            if (!ChatClients.ContainsKey(name))
            {
                Console.WriteLine($"++ {name} logged in");
                // 目前登录的其他客户端(要在添加本客户端前拿到,不然就加上自己了)
                List<User> loginedUsers = new List<User>(ChatClients.Values);
                User newUser = new User { Name = name, ID = Context.ConnectionId, Photo = photo };
                if (!ChatClients.TryAdd(name, newUser))
                {
                    return null;
                }
                // 保存自己的用户名, 后续用来区别和唯一标识自己
                Clients.CallerState.UserName = name;
                // 通知其他客户端, 本客户端已登录
                Clients.Others.ParticipantLogin(newUser);
                // 把所有[其他]客户端信息, 返回给调用客户端, 以便显示登录列表
                return loginedUsers;
            }
            return null;
        }

        public void Logout()
        {
            var name = Clients.CallerState.UserName;
            if (!string.IsNullOrEmpty(name))
            {
                User client = new User();
                ChatClients.TryRemove(name, out client);
                Clients.Others.ParticipantLogout(name);
                Console.WriteLine($"-- {name} logged out");
            }
        }

        public void BroadcastTextMessage(string message)
        {
            var name = Clients.CallerState.UserName;
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(message))
            {
                Clients.Others.BroadcastTextMessage(name, message);
            }
        }

        public void BroadcastImageMessage(byte[] img)
        {
            var name = Clients.CallerState.UserName;
            if (img != null)
            {
                Clients.Others.BroadcastPictureMessage(name, img);
            }
        }

        public void UnicastTextMessage(string recepient, string message)
        {
            var sender = Clients.CallerState.UserName;
            if (!string.IsNullOrEmpty(sender) && recepient != sender &&
                !string.IsNullOrEmpty(message) && ChatClients.ContainsKey(recepient))
            {
                // 获取私聊到的客户端信息
                if (ChatClients.TryGetValue(recepient, out var client))
                {
                    /// Client为什么用的是<see cref="User.ID"/>字段? 并没有告诉客户端这个作为连接的id呀
                    /// todo:
                    Clients.Client(client.ID).UnicastTextMessage(sender, message);

                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recepient"></param>
        /// <param name="img"></param>
        public void UnicastImageMessage(string recepient, byte[] img)
        {
            var sender = Clients.CallerState.UserName;
            if (!string.IsNullOrEmpty(sender) && recepient != sender &&
                img != null && ChatClients.ContainsKey(recepient))
            {
                if (ChatClients.TryGetValue(recepient, out var client))
                {
                    Clients.Client(client.ID).UnicastPictureMessage(sender, img);
                }
            }
        }

        /// <summary>
        /// 打字通知
        /// </summary>
        /// <param name="lookTypingUserName">正在[被]接收打字的人(即: 并没有打字, 正在看对面打字的人. 正在打字的人通知在发给谁)</param>
        public void Typing(string lookTypingUserName)
        {
            if (string.IsNullOrEmpty(lookTypingUserName)) return;
            // 发送消息的人
            var sender = Clients.CallerState.UserName;

            if (ChatClients.TryGetValue(lookTypingUserName, out var lookTypingUser))
            {
                // 通知"围观打字人", 打字人的用户名 
                Clients.Client(lookTypingUser.ID) // 围观者
                    .ParticipantTyping(sender); // 打字者的用户名
            };
        }
    }
}