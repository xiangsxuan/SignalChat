using Unity;
using ChatClientCS.Services;
using ChatClientCS.ViewModels;

namespace ChatClientCS.Utils
{
    /// <summary>
    /// Ioc容器 与 实例化映射配置. 放在程序App全局
    /// todo: 这里放在App全局, 程序其他位置就可以用了吗?
    /// </summary>
    public class ViewModelLocator
    {
        private UnityContainer container;

        public ViewModelLocator()
        {
            container = new UnityContainer();
            container.RegisterType<IChatService, ChatService>();
            container.RegisterType<IDialogService, DialogService>();
        }

        public MainWindowViewModel MainVM
        {
            get { return container.Resolve<MainWindowViewModel>(); }
        }
    }
}
