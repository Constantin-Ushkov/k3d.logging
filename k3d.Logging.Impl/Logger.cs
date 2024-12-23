using Ice.Core.Diagnostics;
using k3d.Logging.Interface;

namespace k3d.Logging.Impl
{
    internal class Logger: ILogger 
    {
        public string Module { get; }
        public string Topic { get; }

        public Logger(ILoggingService service, string module, string topic)
        {
            Assert.Argument.IsNotNull(service, nameof(service));
            Assert.Argument.IsNotNull(module, nameof(module));
            Assert.Argument.IsNotNull(topic, nameof(topic));
            
            Module = module;
            Topic = topic;

            _service = service;
        }

        public void Verbose(string message, params object[] args)
            => _service.Verbose(Module, Topic, message, args);

        public void Debug(string message, params object[] args)
            => _service.Debug(Module, Topic, message, args);

        public void Info(string message, params object[] args)
            => _service.Info(Module, Topic, message, args);

        public void Warning(string message, params object[] args)
            => _service.Warning(Module, Topic, message, args);

        public void Error(string message, params object[] args)
            => _service.Error(Module, Topic, message, args);

        public void Fatal(string message, params object[] args)
            => _service.Fatal(Module, Topic, message, args);

        private readonly ILoggingService _service;
    }
}