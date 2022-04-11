namespace ChatClientCS.Services
{
    /// <summary>
    /// 弹窗服务
    /// 为什么这个也单独做成接口? 考虑以后可能有集中的其他实现? 比如接入一个新的通知组件?
    /// </summary>
    public interface IDialogService
    {
        void ShowNotification(string message, string caption = "");
        bool ShowConfirmationRequest(string message, string caption = "");
        string OpenFile(string caption, string filter = @"All files (*.*)|*.*");
    }
}