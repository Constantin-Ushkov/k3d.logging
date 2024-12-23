using k3d.Logging.Interface;

namespace k3d.Logging.Tcp
{
    internal interface IMessageHistoryInternal: IMessageHistory, IDisposable
    {
        new IMessageFilterInternal Filter { get; }

        void Start();
        
        void AddMessage(LogMessageDto dto);
        void AddMessageInternal(Severity severity, string message);
    }
}