using Ice.Core.Reporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ice.TcpLogger
{
    internal interface IMessageSerializer
    {
        byte[] SerializeMessage(LogMessageDto message);
        LogMessageDto DeserializeMessage(byte[] bytes);
    }
}
