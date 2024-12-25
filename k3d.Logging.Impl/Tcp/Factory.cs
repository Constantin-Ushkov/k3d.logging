using System.Net.Sockets;

namespace k3d.Logging.Impl.Tcp
{
    public class Factory: IFactory
    {
        public static ILoggingClient CreateClient(ILoggingClientConfiguration configuration)
            => new LoggingClient(configuration);

        public static ILoggingServer CreateServer(ILoggingServerConfiguration configuration)
            => new LoggingServer(configuration);
        
        IMessageHistoryInternal IFactory.CreateMessageHistory()
            => new MessageHistory(this);

        IMessageFilterInternal IFactory.CreateMessageFilter()
            => new MessageFilter();
        
        IClientListener IFactory.CreateClientListener(Socket socket, IMessageHistoryInternal history)
            => new ClientListener(socket, history);
    }
}