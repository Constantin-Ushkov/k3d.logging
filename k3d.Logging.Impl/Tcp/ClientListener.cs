using System.Net.Sockets;
using System.Text;
using k3d.Common.Diagnostics;
using k3d.Logging.Interface;

namespace k3d.Logging.Impl.Tcp
{
    internal class ClientListener: IClientListener
    {
        public string Name
        {
            get; private set; // todo: ...
        }
        
        public ClientListener(Socket socket, IMessageHistoryInternal history, IMessageSerializer serializer)
        {
            Assert.Argument.IsNotNull(socket, nameof(socket));
            Assert.Argument.IsNotNull(history, nameof(history));
            Assert.Argument.IsNotNull(serializer, nameof(serializer));

            _socket = socket;
            _history = history;
            _serializer = serializer;
            
            StartThread();
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            
            StopThread();

            _disposed = true;
        }

        private void ThreadMethod()
        {
            // todo: getting client name (and other info)
            
            while (true)
            {
                var result = _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, Receive, null);

                if (WaitHandle.WaitAny(new[] {_stopEvent, result.AsyncWaitHandle}) == 0)
                {
                    _history.AddMessageInternal(Severity.Verbose, $"(Name): Stop event has been set, exiting.");
                    
                    // todo: cancel receive or something like this
                    
                    break;
                }
            }
        }

        private void StartThread()
        {
            if (_thread is not null)
            {
                return;
            }
            
            _thread = new Thread(ThreadMethod);
            _thread.Start();
        }

        private void StopThread()
        {
            if (_thread is null)
            {
                return;
            }
            
            _stopEvent.Set();

            if (!_thread.Join(TimeSpan.FromSeconds(5)))
            {
                _thread.Interrupt();
            }

            _thread.Join();
            _thread = null;
        }

        private void Receive(IAsyncResult result)
        {
            /*
            while(true)
            {
                var buffer = new byte[1_024];
                var received = handler.Receive(buffer, SocketFlags.None);
                var response = Encoding.UTF8.GetString(buffer, 0, received);

                var eom = "<|EOM|>";
                if (response.IndexOf(eom) > -1)
                {
                    Console.WriteLine($"Message received: {response.Replace(eom, String.Empty)}");

                    var ackMessage = "<|ACK|>";
                    var echoBytes = Encoding.UTF8.GetBytes(ackMessage);

                    handler.Send(echoBytes, 0);

                    //
                }
            }
            */

            try
            {
                var byteCount = _socket.EndReceive(result);

                var message = _serializer.DeserializeMessage(_buffer);
            
                SendReceiveAcknowledgment();
            
                _history.AddMessage(message);
            }
            catch (Exception e)
            {
                _history.AddMessageInternal(Severity.Error, $"Failed to finish receive message. Exception: {e}.");
            }
        }
        
        private void SendReceiveAcknowledgment()
            => _socket.Send(_ackBytes, 0);

        private bool _disposed;
        private readonly Socket _socket;
        private readonly IMessageSerializer _serializer;
        private readonly IMessageHistoryInternal _history;
        private Thread? _thread;
        private readonly ManualResetEvent _stopEvent = new (false);
        private readonly byte[] _buffer = new byte[10_240];
        private readonly byte[] _ackBytes = Encoding.UTF8.GetBytes("<|ACK|>");
    }
}