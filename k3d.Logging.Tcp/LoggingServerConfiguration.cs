
namespace k3d.Logging.Tcp
{
    public class LoggingServerConfiguration : ILoggingServerConfiguration
    {
        public uint Port { get; set; } = 11_000;
        public uint SocketBacklog { get; set; } = 1_000;
    }
}
