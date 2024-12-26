using k3d.Logging.Impl.Tcp;
using k3d.Logging.Interface;
using System.Net.Sockets;

namespace k3d.Logging.Impl
{
    internal interface IFactory
    {
        ITcpLogger CreateTcpLogger(ILoggingClientConfiguration configuration, IMessageSerializer? serializer);
        IMessageDtoSerializer CreateMessageDtoSerializer();
        IMessageSerializer CreateMessageSerializer(IMessageDtoSerializer? messageDtoSerializer = null);

        IMessageHistoryInternal CreateMessageHistory();
        IMessageFilterInternal CreateMessageFilter();
        IClientListener CreateClientListener(Socket socket, IMessageHistoryInternal history, IMessageSerializer? serializer = null);
    }
}
