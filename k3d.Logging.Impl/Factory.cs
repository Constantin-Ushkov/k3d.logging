using k3d.Logging.Impl.Tcp;
using k3d.Logging.Interface;
using System.Net.Sockets;

namespace k3d.Logging.Impl
{
    public class Factory: IFactory
    {
        public Factory()
        {
            _ifactory = this;
        }

        public static ILoggingService CreateLoggingService()
            => new LoggingService();

        public static ILoggingServer CreateTcpServer(ILoggingServerConfiguration configuration)
            => new LoggingServer(configuration);

        ITcpLogger IFactory.CreateTcpLogger(ILoggingClientConfiguration configuration, IMessageSerializer? serializer)
            => new TcpLogger(configuration, serializer ?? _ifactory.CreateMessageSerializer());

        IMessageDtoSerializer IFactory.CreateMessageDtoSerializer()
            => new MessageDtoSerializer();

        IMessageSerializer IFactory.CreateMessageSerializer(IMessageDtoSerializer? messageDtoSerializer)
            => new ProtocolFormatter(messageDtoSerializer ?? _ifactory.CreateMessageDtoSerializer());

        IMessageHistoryInternal IFactory.CreateMessageHistory()
            => new MessageHistory(this);

        IMessageFilterInternal IFactory.CreateMessageFilter()
            => new MessageFilter();

        IClientListener IFactory.CreateClientListener(Socket socket, IMessageHistoryInternal history, IMessageSerializer? serializer)
            => new ClientListener(socket, history, serializer ?? _ifactory.CreateMessageSerializer());

        private readonly IFactory _ifactory;
    }
}
