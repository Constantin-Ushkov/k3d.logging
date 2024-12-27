using k3d.Logging.Impl.Tcp;
using k3d.Logging.Interface;
using System.Net.Sockets;

namespace k3d.Logging.Impl
{
    internal interface IFactory
    {
        ITcpWriter CreateTcpLogger(ILoggingClientConfiguration configuration, IMessageSerializer? serializer);
        IMessageDtoSerializer CreateMessageDtoSerializer();
        IMessageSerializer CreateMessageSerializer(IMessageDtoSerializer? messageDtoSerializer = null);

        ILoggerCollection CreateLoggerCollection(ILoggingService service);
        IOutputWriterCollection CreateOutputWriterCollection();

        IMessageHistoryInternal CreateMessageHistory();
        IMessageFilterInternal CreateMessageFilter();
        IClientListener CreateClientListener(Socket socket, IMessageHistoryInternal history, IMessageSerializer? serializer = null);
    }
}
