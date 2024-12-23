using System.Net;
using System.Net.Sockets;
using System.Text;
using Ice.Core.Reporting;

namespace Ice.TcpLogger
{
    internal class LoggingClient: ILoggingClient
    {
        public LoggingClient(ILoggingClientConfiguration configuration, IMessageSerializer? serializer = null)
        {
            _configuration = configuration
                ?? throw new ArgumentNullException(nameof(configuration));

            _serializer = serializer
                ?? new MessageSerializer();
            
            _thread = new Thread(ThreadMethod);
            _thread.Start();
        }
        
        public void Message(LogMessageDto message)
        {
            Enqueue(message);
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _threadStopEvent.Set();

            if (!_thread.Join(TimeSpan.FromSeconds(5)))
            {
                _thread.Interrupt();
            }

            _disposed = true;
        }

        private void ThreadMethod()
        {
            var handles = new WaitHandle[] {_threadStopEvent, _newMessageEvent};

            try
            {
                while (true)
                {
                    if (WaitHandle.WaitAny(handles, TimeSpan.FromSeconds(1)) == 0)
                    {
                        break;
                    }

                    SendAllMessages();
                }
            }
            catch (ThreadInterruptedException)
            {
            }
            
            Disconnect();
        }

        private void SendAllMessages()
        {
            while (PeekMessage(out var message))
            {
                if (!IsConnected() && !Connect())
                {
                    break;
                }
                    
                if (!SendMessage(message!))
                {
                    break;
                }
                    
                Dequeue();
            }
        }

        private void Enqueue(LogMessageDto dto)
        {
            lock (_queueSyncObject)
            {
                if (_queue.Count >= _configuration.QueueSizeLimit)
                {
                    _queue.Dequeue();
                }
                
                _queue.Enqueue(dto);
            }
        }

        private LogMessageDto? Dequeue()
        {
            lock (_queueSyncObject)
            {
                return _queue.Count > 0 ? _queue.Dequeue() : null;
            }
        }

        private bool PeekMessage(out LogMessageDto? message)
        {
            lock (_queueSyncObject)
            {
                message = _queue.FirstOrDefault();
                return _queue.Any();
            }
        }

        private bool IsConnected() => 
            _socket != null;
        
        private bool Connect()
        {
            if (_socket is not null)
            {
                return true;
            }

            try
            {
                var hostName = _configuration.ServerHost ?? Dns.GetHostName();
                var hostEntry = Dns.GetHostEntry(hostName);
                var address = hostEntry.AddressList[0];
                var endpoint = new IPEndPoint(address, (int)_configuration.ServerPort);
                
                _socket = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                _socket.Connect(endpoint);
            }
            catch (SocketException)
            {
                _socket?.Dispose();
                _socket = null;
            }

            return _socket is not null;
        }

        private void Disconnect()
        {
            if (_socket is null)
            {
                return;
            }
            
            _socket.Disconnect(false);
            _socket.Dispose();
        }

        private bool SendMessage(LogMessageDto dto)
        {
            var message = _serializer.SerializeMessage(dto);
            
            for (var i = 0; i < _configuration.SendAttemptCount; ++i)
            {
                if (SendMessage(message))
                {
                    return true;
                }
                
                Thread.Sleep(_configuration.SendErrorTimeout);
            }

            return false;
        }

        private bool SendMessage(byte[] message)
        {
            try
            {
                _socket!.Send(message);
            
                var buffer = new byte[1_024];
                var received = _socket.Receive(buffer);
                var response = Encoding.UTF8.GetString(buffer, 0, received);
                
                // todo: ordinal to prevent duplicate messages on resend

                return response == "<|ACK|>";
            }
            catch (SocketException)
            {
            }

            return false;
        }
        
        private bool _disposed;
        private readonly ILoggingClientConfiguration _configuration;
        private readonly IMessageSerializer _serializer;
        private readonly Thread _thread;
        private readonly ManualResetEvent _threadStopEvent = new (false);
        private readonly AutoResetEvent _newMessageEvent = new (false);
        private readonly object _queueSyncObject = new();
        private readonly Queue<LogMessageDto> _queue = new();
        private Socket? _socket;
    }
}