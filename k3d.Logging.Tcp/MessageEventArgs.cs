using Ice.Core.Reporting;

namespace Ice.TcpLogger
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