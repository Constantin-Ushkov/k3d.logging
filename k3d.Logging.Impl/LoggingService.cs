using k3d.Logging.Interface;

namespace k3d.Logging.Impl
{
    public sealed class LoggingService : ILoggingService, IDisposable
    {
        #region Properties

        public ILoggerCollection Loggers { get; }
        public IOutputWriterCollection OutputWriters { get; }

        internal IFactory Factory { get; }
        public Severity Filter { get; set; } = Severity.Debug;

        #endregion // Properties

        #region Constructors/Finalizers

        public LoggingService() :
            this(null)
        {
        }

        internal LoggingService(IFactory? factory)
        {
            Factory = factory ?? new Factory();

            Loggers = Factory.CreateLoggerCollection(this);
            OutputWriters = Factory.CreateOutputWriterCollection();
        }

        #endregion // Constructors/Finalizers

        #region Public Methods

        public void Init()
        {
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

            var messageDto = new LogMessageDto(_messageCounter++, module, severity, topic, message, DateTime.Now, args);

            foreach(var writer in OutputWriters)
            {
                writer.WriteMessage(messageDto);
            }
        }

        #endregion // Public Methods

        #region IDisposable Implementation

        public void Dispose()
        {
            // no unmanaged resources - no need for generic dispose pattern
            OutputWriters.Dispose();
        }

        #endregion // IDisposable Implementation

        #region Fields

        private uint _messageCounter;

        #endregion // Fields
    }
}
