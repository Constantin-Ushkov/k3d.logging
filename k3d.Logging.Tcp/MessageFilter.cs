using k3d.Logging.Interface;
using Ice.Core.Utilities;

namespace k3d.Logging.Tcp
{
    public class MessageFilter: IMessageFilterInternal
    {
        public IList<string> Modules { get; }
        public IList<string> Topics { get; }

        public Severity? Severity
        {
            get
            {
                lock (_syncObject)
                {
                    return _severity;
                }
            }

            set
            {
                lock (_syncObject)
                {
                    _severity = value;
                }
            }
        }

        public MessageFilter()
        {
            Modules = new SyncList<string>(_syncObject);
            Topics = new SyncList<string>(_syncObject);
        }
        
        public void Set(IEnumerable<string> modules, IEnumerable<string> topics, Severity? severity)
        {
            lock (_syncObject)
            {
                Modules.Clear();
                Modules.AddRange(modules);
            
                Topics.Clear();
                Topics.AddRange(topics);

                Severity = severity;
            }
        }

        public void Reset()
        {
            lock (_syncObject)
            {
                Modules.Clear();
                Topics.Clear();
                Severity = null;
            }
        }

        public bool Pass(LogMessageDto message)
        {
            lock (_syncObject)
            {
                if (Modules.Any() && !Modules.Contains(message.Module))
                {
                    return false;
                }
            
                if (Topics.Any() && !Topics.Contains(message.Module))
                {
                    return false;
                }

                if (Severity.HasValue && Severity.Value > message.Severity)
                {
                    return false;
                }
            }

            return true;
        }

        private readonly object _syncObject = new();
        private Severity? _severity;
    }
}