using k3d.Common.Diagnostics;
using k3d.Logging.Interface;

namespace k3d.Logging.Tcp
{
    internal class MessageHistory: IMessageHistoryInternal
    {
        IMessageFilter IMessageHistory.Filter => Filter;
        
        public event EventHandler<MessageEventArgs>? Message;
        public event EventHandler<MessageEventArgs>? InternalMessage;

        public IMessageFilterInternal Filter { get; }

        public MessageHistory(IFactory factory)
        {
            Assert.Argument.IsNotNull(factory, nameof(factory));
            
            Filter = factory.CreateMessageFilter();
        }

        public void Start()
        {
            if (_processingThread is not null)
            {
                return;
            }

            _processingThread = new Thread(NotificationThread);
            _processingThread.Start();
        }

        public IEnumerable<LogMessageDto> GetFilteredMessages()
        {
            lock (_historySyncObject)
            {
                return _history.Where(Filter.Pass).ToArray();
            }
        }
        
        public void AddMessage(LogMessageDto dto)
        {
            lock (_incomingMessageSyncObject)
            {
                _incomingMessageQueue.Enqueue(dto);
                _messageEvent.Set();
            }
        }

        public void AddMessageInternal(Severity severity, string message)
            => AddMessage(new LogMessageDto(0, Constants.ModuleName, severity, Constants.Topic, message, DateTime.Now, null));

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            
            StopProcessingThread();
            _disposed = true;
        }

        private void NotificationThread()
        {
            try
            {
                var events = new WaitHandle[] {_stopEvent, _messageEvent};
                
                while (true)
                {
                    if (WaitHandle.WaitAny(events) == 0)
                    {
                        break;
                    }

                    ProcessIncomingMessages();
                }
            }
            catch (ThreadInterruptedException)
            {
            }
            catch (ThreadAbortException)
            {
            }
        }

        private void StopProcessingThread()
        {
            if (_processingThread is null)
            {
                return;
            }
            
            _stopEvent.Set();

            if (!_processingThread.Join(TimeSpan.FromSeconds(5)))
            {
                _processingThread.Interrupt();
            }

            _processingThread.Join();
            _processingThread = null;
        }

        private void ProcessIncomingMessages()
        {
            while (PeekIncomingMessage(out var message))
            {
                NotifyMessage(message!);
                AddHistoryMessage(message!);
                DequeueIncomingMessage();
            }
        }

        private bool PeekIncomingMessage(out LogMessageDto? message)
        {
            lock (_incomingMessageSyncObject)
            {
                message = _incomingMessageQueue.FirstOrDefault();
            }

            return message is not null;
        }

        private void NotifyMessage(LogMessageDto message)
        {
            if (!Filter.Pass(message))
            {
                return;
            }
            
            try
            {
                if (IsInternalMessage(message))
                {
                    InternalMessage?.Invoke(this, new MessageEventArgs(message!));
                }
                else
                {
                    Message?.Invoke(this, new MessageEventArgs(message!));
                }
            }
            catch (Exception e)
            {
                AddMessageInternal(Severity.Error,
                    $"Failed to notify incoming message, exception: {e}.");
            }
        }

        private static bool IsInternalMessage(LogMessageDto message)
            => message.Module == Constants.ModuleName;

        private void AddHistoryMessage(LogMessageDto message)
        {
            lock (_historySyncObject)
            {
                _history.Add(message);
            }
        }

        private void DequeueIncomingMessage()
        {
            lock (_incomingMessageSyncObject)
            {
                _incomingMessageQueue.Dequeue();
            }
        }

        private bool _disposed;
        private readonly object _historySyncObject = new();
        private readonly List<LogMessageDto> _history = new();
        private readonly object _incomingMessageSyncObject = new();
        private readonly Queue<LogMessageDto> _incomingMessageQueue = new();
        private Thread? _processingThread;
        private readonly AutoResetEvent _stopEvent = new(false);
        private readonly AutoResetEvent _messageEvent = new(false);
    }
}