namespace Ice.TcpLogger
{
    public interface ILoggingServerConfiguration
    {
        uint Port { get; }
        uint SocketBacklog { get; }
    }
}