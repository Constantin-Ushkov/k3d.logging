using Ice.Core.Reporting;

namespace Ice.TcpLogger
{
    internal interface IMessageFilterInternal: IMessageFilter
    {
        bool Pass(LogMessageDto message);
    }
}