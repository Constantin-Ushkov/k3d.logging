using k3d.Logging.Interface;

namespace k3d.Logging.Impl.Tcp
{
    internal interface IMessageFilterInternal: IMessageFilter
    {
        bool Pass(LogMessageDto message);
    }
}