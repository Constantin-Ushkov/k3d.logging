using k3d.Logging.Interface;

namespace k3d.Logging.Tcp
{
    internal interface IMessageFilterInternal: IMessageFilter
    {
        bool Pass(LogMessageDto message);
    }
}