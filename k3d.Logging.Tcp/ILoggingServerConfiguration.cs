namespace k3d.Logging.Tcp
{
    public interface ILoggingServerConfiguration
    {
        uint Port { get; }
        uint SocketBacklog { get; }
    }
}