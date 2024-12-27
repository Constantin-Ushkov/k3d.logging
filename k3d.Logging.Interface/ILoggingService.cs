
namespace k3d.Logging.Interface
{
    public interface ILoggingService
    {
        IOutputWriterCollection OutputWriters { get; }
        ILoggerCollection Loggers { get; }

        void Verbose(string module, string topic, string message, params object[] args);
        void Debug(string module, string topic, string message, params object[] args);
        void Info(string module, string topic, string message, params object[] args);
        void Warning(string module, string topic, string message, params object[] args);
        void Error(string module, string topic, string message, params object[] args);
        void Fatal(string module, string topic, string message, params object[] args);
    }
}
