using Ice.Core.Reporting;
using System.Runtime.Serialization;
using System.Text;

namespace Ice.TcpLogger
{
    internal class MessageSerializer : IMessageSerializer // todo: ProtocolFormatter ? also serialize\deserialize ACK here
    {
        public LogMessageDto DeserializeMessage(byte[] bytes)
        {
            if (bytes is null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            if (bytes.Length < Constants.MinMessageLength)
            {
                throw new ArgumentException(nameof(bytes),
                    $"Failed to deserialize log message. Minimum expected length " +
                    $"is {Constants.MinMessageLength}, actual: {bytes.Length}.");
            }

            if (!CheckSignature(bytes, _messageSignature))
            {
                throw new SerializationException(
                    $"Failed to deserialize log message. Wrong message signature. " +
                    $"Expected: {Constants.Signatures.Message}, " +
                    $"got: {Encoding.UTF8.GetString(bytes, 0, _messageSignature.Length)}");
            }

            var length = BitConverter.ToInt32(bytes, _messageSignature.Length);

            if (length > (bytes.Length - Constants.Signatures.Message.Length - sizeof(int)))
            {
                throw new SerializationException(
                    $"Failed to deserialize log message. Message size is too big: " +
                    $"{length}, whole data size is: {bytes.Length}.");
            }

            return LogMessageDto.FromByteArray(bytes, _messageSignature.Length + sizeof(int), length);
        }

        public byte[] SerializeMessage(LogMessageDto dto)
        {
            dto.MakeMessageStringFormatted();

            var bytes = dto.ToByteArray();
            using var memory = new MemoryStream();

            memory.Write(_messageSignature);
            memory.Write(BitConverter.GetBytes(bytes.Length));
            memory.Write(bytes);

            return memory.ToArray();
        }

        private static bool CheckSignature(byte[] bytes, byte[] signature)
        {
            if (bytes.Length < signature.Length)
            {
                return false;
            }

            for (var i=0; i < signature.Length; i++)
            {
                if (bytes[i] != signature[i])
                {
                    return false;
                }
            }

            return true;
        }

        private readonly byte[] _messageSignature
            = Encoding.UTF8.GetBytes(Constants.Signatures.Message);
    }
}
