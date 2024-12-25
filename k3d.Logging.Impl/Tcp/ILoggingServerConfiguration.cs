namespace k3d.Logging.Impl.Tcp
{
    public interface ILoggingServerConfiguration
    {
        uint Port { get; }
        uint SocketBacklog { get; }
    }
}