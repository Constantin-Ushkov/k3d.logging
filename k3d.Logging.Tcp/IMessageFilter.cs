using k3d.Logging.Interface;

namespace k3d.Logging.Tcp
{
    public interface IMessageFilter
    {
        IList<string> Modules { get; }
        IList<string> Topics { get; }
        Severity? Severity { get; set; }
        
        void Set(IEnumerable<string> modules, IEnumerable<string> topics, Severity? severity);
        void Reset();
    }
}

