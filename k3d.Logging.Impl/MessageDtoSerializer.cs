using k3d.Logging.Interface;
using System.Runtime.CompilerServices;
using System.Text.Json;

[assembly: InternalsVisibleTo("k3d.Logging.Test")]

namespace k3d.Logging.Impl
{
    internal class MessageDtoSerializer : IMessageDtoSerializer
    {
        public LogMessageDto Deserialize(byte[] bytes, uint offset, uint count)
            => Deserialize(bytes, (int)offset, (int)count)!;

        public LogMessageDto Deserialize(byte[] bytes, int offset, int count)
            => JsonSerializer.Deserialize<LogMessageDto>(new ReadOnlySpan<byte>(bytes, offset, count))!;

        public byte[] Serialize(LogMessageDto message)
        {
            message.MakeMessageStringFormatted();
            return JsonSerializer.SerializeToUtf8Bytes(message);
        }
    }
}
