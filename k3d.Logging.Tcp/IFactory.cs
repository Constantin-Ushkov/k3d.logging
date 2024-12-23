using System.Net.Sockets;

namespace Ice.TcpLogger
{
    internal interface IFactory
    {
        IMessageHistoryInternal CreateMessageHistory();
        IMessageFilterInternal CreateMessageFilter();
        IClientListener CreateClientListener(Socket socket, IMessageHistoryInternal history);
    }
}