using System.Net.Sockets;

namespace k3d.Logging.Tcp
{
    internal interface IFactory
    {
        IMessageHistoryInternal CreateMessageHistory();
        IMessageFilterInternal CreateMessageFilter();
        IClientListener CreateClientListener(Socket socket, IMessageHistoryInternal history);
    }
}