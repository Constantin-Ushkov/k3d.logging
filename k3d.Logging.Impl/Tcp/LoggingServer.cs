using System.Net;
using System.Net.Sockets;
using k3d.Logging.Interface;

namespace k3d.Logging.Impl.Tcp
{
    public sealed class LoggingServer: ILoggingServer
    {
        public IMessageHistory Messages => _history;

        public LoggingServer(ILoggingServerConfiguration configuration):
            this(configuration, new Factory())
        {
        }
        
        internal LoggingServer(ILoggingServerConfiguration configuration, IFactory factory)
        {
            _configuration = configuration
                ?? throw new ArgumentNullException(nameof(configuration));
            
            _factory = factory
                ?? throw new ArgumentNullException(nameof(factory));

            _history = _factory.CreateMessageHistory();
        }

        public void Start()
        {
            StartConnectionListening();
            _history.Start();
        }
        
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            
            StopConnectionListening();
            StopClientListeners();
            
            _history.Dispose();
            _disposed = true;
        }

        private void StartConnectionListening()
        {
            if (_connectionListeningThread is not null)
            {
                return;
            }

            _history.AddMessageInternal(Severity.Verbose, "Starting connection listening thread...");
            
            _connectionListeningThread = new Thread(ConnectionListeningThread);
            _connectionListeningThread.Start();
            
            _history.AddMessageInternal(Severity.Verbose, "Connection listening thread has been started.");
        }
        
        private void StopConnectionListening()
        {
            _history.AddMessageInternal(Severity.Verbose, "Stopping connection listening thread...");
            _threadStopEvent.Set();

            if (!_connectionListeningThread!.Join(TimeSpan.FromSeconds(5)))
            {
                _connectionListeningThread!.Interrupt();
            }
            
            _history.AddMessageInternal(Severity.Verbose, "Connection listening thread has been stopped.");
        }
        
        private void ConnectionListeningThread()
        {
            _history.AddMessageInternal(Severity.Verbose, "Entering connection listening thread...");
            
            OpenSocket();
            
            try
            {
                while (true)
                {
                    var result = _connectionListenerSocket!.BeginAccept(CreateClientListener, null);

                    if (WaitHandle.WaitAny(new[] {_threadStopEvent, result.AsyncWaitHandle}) == 0)
                    {
                        _history.AddMessageInternal(Severity.Verbose, "Connection listening thread stop event has been set, exiting.");
                        break;
                    }
                }
            }
            catch (ThreadInterruptedException)
            {
                _history.AddMessageInternal(Severity.Verbose, "Connection listening thread has ben interrupted.");
            }
            
            CloseSocket();
            
            _history.AddMessageInternal(Severity.Verbose, "Leaving connection listening thread.");
        }

        private void OpenSocket()
        {
            _history.AddMessageInternal(Severity.Verbose, "Opening connection listening socket...");
            
            var hostName = Dns.GetHostName();
            var hostEntry = Dns.GetHostEntry(hostName);
            var address = hostEntry.AddressList[0];
            var endpoint = new IPEndPoint(address, (int) _configuration.Port);

            _connectionListenerSocket = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            
            _connectionListenerSocket.Bind(endpoint);
            _connectionListenerSocket.Listen((int)_configuration.SocketBacklog);
            
            _history.AddMessageInternal(Severity.Verbose, "Connection listening thread has been opened.");
        }

        private void CloseSocket()
        {
            _history.AddMessageInternal(Severity.Verbose, "Closing connection listening socket...");
            
            _connectionListenerSocket!.Shutdown(SocketShutdown.Receive);
            _connectionListenerSocket!.Dispose();
            
            _history.AddMessageInternal(Severity.Verbose, "Connection listening thread has been closed.");
        }

        private void CreateClientListener(IAsyncResult result)
        {
            _history.AddMessageInternal(Severity.Verbose, "Adding new client listener...");
            
            var socket = _connectionListenerSocket!.EndAccept(result);
            _clientListeners.Add(_factory.CreateClientListener(socket, _history));
            
            _history.AddMessageInternal(Severity.Verbose, "New client listener has been added.");
        }

        private void StopClientListeners()
        {
            _history.AddMessageInternal(Severity.Verbose, "Stopping client listeners...");
            
            foreach (var listener in _clientListeners)
            {
                listener.Dispose();
            }
            
            _history.AddMessageInternal(Severity.Verbose, "Client listeners has been stopped.");
        }

        private bool _disposed;
        private readonly IFactory _factory;
        private readonly ILoggingServerConfiguration _configuration;
        private readonly IMessageHistoryInternal _history;
        private Thread? _connectionListeningThread;
        private readonly ManualResetEvent _threadStopEvent = new (false);
        private Socket? _connectionListenerSocket;
        private readonly List<IClientListener> _clientListeners = new();
    }
}