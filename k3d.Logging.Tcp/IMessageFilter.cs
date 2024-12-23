using Ice.Core.Reporting;

namespace Ice.TcpLogger
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

