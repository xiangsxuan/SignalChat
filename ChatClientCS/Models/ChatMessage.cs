using System;

namespace ChatClientCS.Models
{
    /// <summary>
    /// 聊天消息
    /// </summary>
    public class ChatMessage
    {
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
        public string Author { get; set; }
        public DateTime Time { get; set; }
        public string Picture { get; set; }
        /// <summary>
        /// 是否自己发送的消息
        /// </summary>
        public bool IsOriginNative { get; set; }
    }
}
