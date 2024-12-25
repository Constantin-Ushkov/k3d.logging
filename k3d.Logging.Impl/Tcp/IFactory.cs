using System.Net.Sockets;

namespace k3d.Logging.Impl.Tcp
{
    internal interface IFactory
    {
        IMessageHistoryInternal CreateMessageHistory();
        IMessageFilterInternal CreateMessageFilter();
        IClientListener CreateClientListener(Socket socket, IMessageHistoryInternal history);
    }
}