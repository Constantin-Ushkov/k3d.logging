using Ice.Core.Reporting;

namespace Ice.TcpLogger
{
    public interface IMessageHistory
    {
        event EventHandler<MessageEventArgs>? Message;
        event EventHandler<MessageEventArgs>? InternalMessage;

        IMessageFilter Filter { get; }
        
        IEnumerable<LogMessageDto> GetFilteredMessages();
    }
}