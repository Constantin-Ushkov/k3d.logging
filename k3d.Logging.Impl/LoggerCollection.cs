using k3d.Common.Diagnostics;
using k3d.Logging.Interface;
using System.Collections;

namespace k3d.Logging.Impl
{
    internal class LoggerCollection : ILoggerCollection
    {
        public LoggerCollection(ILoggingService service)
        {
            Assert.Argument.IsNotNull(service, nameof(service));
            _service = service;
        }

        public ILogger GetLogger(string module, string topic)
        {
            var name = $"{module}\\{topic}";

            if (_loggers.TryGetValue(name, out var logger))
            {
                return logger;
            }

            logger = new Logger(_service, module, topic);
            _loggers.Add(name, logger);

            return logger;
        }

        public IEnumerator<ILogger> GetEnumerator()
            => _loggers.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _loggers.Values.GetEnumerator();

        private readonly ILoggingService _service;
        private readonly Dictionary<string, ILogger> _loggers = [];
    }
}
