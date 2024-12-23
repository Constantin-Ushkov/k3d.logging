using k3d.Logging.Interface;

namespace k3d.Logging.Impl
{
    public class LoggingService : ILoggingService, IDisposable
    {
        #region Properties

        public List<ILoggingListener> Listeners { get; } = [];
        public Severity Filter { get; set; } = Severity.Debug;

        #endregion // Properties

        #region Constructors/Finalizers

        ~LoggingService()
        {
            Dispose(false);
        }

        #endregion // Constructors/Finalizers

        #region Public Methods

        public void Init()
        {
        }

        public ILogger GetLogger(string module, string topic)
        {
            var name = $"{module}\\{topic}";

            if (_loggers.TryGetValue(name, out var logger))
            {
                return logger;
            }

            logger = new Logger(this, module, topic);
            _loggers.Add(name, logger);

            return logger;
        }

        public void Verbose(string module, string topic, string message, params object[] args) =>
            Message(Severity.Verbose, module, topic, message, args);

        public void Debug(string module, string topic, string message, params object[] args) =>
            Message(Severity.Debug, module, topic, message, args);

        public void Info(string module, string topic, string message, params object[] args) =>
            Message(Severity.Info, module, topic, message, args);

        public void Warning(string module, string topic, string message, params object[] args) =>
            Message(Severity.Warning, module, topic, message, args);

        public void Error(string module, string topic, string message, params object[] args) =>
            Message(Severity.Error, module, topic, message, args);

        public void Fatal(string module, string topic, string message, params object[] args) =>
            Message(Severity.Fatal, module, topic, message, args);

        public void Message(Severity severity, string module, string topic, string message, params object[] args)
        {
            if (severity < Filter)
            {
                return;
            }

            var messageDto = new LogMessageDto(_messageCounter++, module, severity, topic, message, args);

            foreach(var listener in Listeners)
            {
                listener.Message(messageDto);
            }
        }

        #endregion // Public Methods

        #region IDisposable Implementation

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                foreach (var listener in Listeners)
                {
                    listener.Dispose();
                }

                Listeners.Clear();
            }

            _disposed = true;
        }

        #endregion // IDisposable Implementation

        #region Fields

        private bool _disposed;
        private uint _messageCounter;
        private readonly Dictionary<string, ILogger> _loggers = new();

        #endregion // Fields
    }
}
