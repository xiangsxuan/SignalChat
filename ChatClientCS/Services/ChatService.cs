using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChatClientCS.Enums;
using System.Net;
using Microsoft.AspNet.SignalR.Client;
using ChartProtocol;

namespace ChatClientCS.Services
{
    public class ChatService : IChatService
    {
        public event Action<string, string, MessageType> NewTextMessageReceived;
        public event Action<string, byte[], MessageType> NewImageMessageReceived;
        public event Action<string> ParticipantDisconnected;
        public event Action<User> ParticipantLoggedIn;
        public event Action<string> ParticipantLoggedOut;
        public event Action<string> ParticipantReconnected;
        public event Action ConnectionReconnecting;
        public event Action ConnectionReconnected;
        public event Action ConnectionClosed;
        public event Action<string> ParticipantTyping;

        private IHubProxy hubProxy;
        private HubConnection connection;
        private string url = "http://localhost:8080/signalchat";

        public async Task ConnectAsync()
        {
            connection = new HubConnection(url);
            hubProxy = connection.CreateHubProxy("ChatHub");
            hubProxy.On<User>("ParticipantLogin", (u) => ParticipantLoggedIn?.Invoke(u));
            hubProxy.On<string>("ParticipantLogout", (n) => ParticipantLoggedOut?.Invoke(n));
            hubProxy.On<string>("ParticipantDisconnection", (n) => ParticipantDisconnected?.Invoke(n));
            hubProxy.On<string>("ParticipantReconnection", (n) => ParticipantReconnected?.Invoke(n));
            hubProxy.On<string, string>("BroadcastTextMessage", (n, m) => NewTextMessageReceived?.Invoke(n, m, MessageType.Broadcast));
            hubProxy.On<string, byte[]>("BroadcastPictureMessage", (n, m) => NewImageMessageReceived?.Invoke(n, m, MessageType.Broadcast));
            hubProxy.On<string, string>("UnicastTextMessage", (n, m) => NewTextMessageReceived?.Invoke(n, m, MessageType.Unicast));
            hubProxy.On<string, byte[]>("UnicastPictureMessage", (n, m) => NewImageMessageReceived?.Invoke(n, m, MessageType.Unicast));
            hubProxy.On<string>("ParticipantTyping", (p) => ParticipantTyping?.Invoke(p));

            connection.Reconnecting += Reconnecting;
            connection.Reconnected += Reconnected;
            connection.Closed += Disconnected;
            //ServicePoint 对象允许的最大并发连接数。对于托管的应用程序，默认 ASP.NET 限制为 10，对于所有其他应用程序，默认连接限制为 2。
            ServicePointManager.DefaultConnectionLimit = 10;
            await connection.Start();
        }

        private void Disconnected()
        {
            ConnectionClosed?.Invoke();
        }

        private void Reconnected()
        {
            ConnectionReconnected?.Invoke();
        }

        private void Reconnecting()
        {
            ConnectionReconnecting?.Invoke();
        }

        public async Task<List<User>> LoginAsync(string name, byte[] photo)
        {
            return await hubProxy.Invoke<List<User>>("Login", new object[] { name, photo });
        }

        public async Task LogoutAsync()
        {
            await hubProxy.Invoke("Logout");
        }

        public async Task SendBroadcastMessageAsync(string msg)
        {
            await hubProxy.Invoke("BroadcastTextMessage", msg);
        }

        public async Task SendBroadcastMessageAsync(byte[] img)
        {
            await hubProxy.Invoke("BroadcastImageMessage", img);
        }

        public async Task SendUnicastMessageAsync(string recepient, string msg)
        {
            await hubProxy.Invoke("UnicastTextMessage", new object[] { recepient, msg });
        }

        public async Task SendUnicastMessageAsync(string recepient, byte[] img)
        {
            await hubProxy.Invoke("UnicastImageMessage", new object[] { recepient, img });
        }

        public async Task TypingAsync(string recepient)
        {
            await hubProxy.Invoke("Typing", recepient);
        }
    }
}