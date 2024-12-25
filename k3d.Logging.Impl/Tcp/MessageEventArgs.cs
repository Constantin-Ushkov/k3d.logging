using k3d.Logging.Interface;

namespace k3d.Logging.Impl.Tcp
{
    public class MessageEventArgs: EventArgs
    {
        public LogMessageDto Message { get; }

        public MessageEventArgs(LogMessageDto dto)
        {
            Message = dto;
        }
    }
}