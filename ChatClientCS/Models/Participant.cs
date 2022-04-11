using ChatClientCS.ViewModels;
using System.Collections.ObjectModel;

namespace ChatClientCS.Models
{
    /// <summary>
    /// 通讯录单个好友信息
    /// </summary>
    public class Participant : ViewModelBase
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public byte[] Photo { get; set; }
        /// <summary>
        /// 消息列表
        /// </summary>
        public ObservableCollection<ChatMessage> Chatter { get; set; }

        private bool _isLoggedIn = true;
        /// <summary>
        /// 登录状态
        /// </summary>
        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set { _isLoggedIn = value; OnPropertyChanged(); }
        }

        private bool _hasSentNewMessage;
        /// <summary>
        /// 是否发送了新信息
        /// </summary>
        public bool HasSentNewMessage
        {
            get { return _hasSentNewMessage; }
            set { _hasSentNewMessage = value; OnPropertyChanged(); }
        }

        private bool _isTyping;
        /// <summary>
        /// 正在打字
        /// </summary>
        public bool IsTyping
        {
            get { return _isTyping; }
            set { _isTyping = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 参与者
        /// </summary>
        public Participant() { Chatter = new ObservableCollection<ChatMessage>(); }
    }
}