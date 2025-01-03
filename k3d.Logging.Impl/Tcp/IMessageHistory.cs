using k3d.Logging.Interface;

namespace k3d.Logging.Impl.Tcp
{
    public interface IMessageHistory
    {
        event EventHandler<MessageEventArgs>? Message;
        event EventHandler<MessageEventArgs>? InternalMessage;

        IMessageFilter Filter { get; }
        
        IEnumerable<LogMessageDto> GetFilteredMessages();
    }
}