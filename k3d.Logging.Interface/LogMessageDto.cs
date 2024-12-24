using System.Text;
using System.Text.Json;

namespace k3d.Logging.Interface
{
    [Serializable]
    public class LogMessageDto
    {
        public uint Ordinal { get; }
        public string Module { get; }
        public Severity Severity  { get; }
        public DateTime CreatedTime { get; } 
        public string Topic { get; }
        public string Message { get; private set; }
        public object[]? Args { get; private set; }

        public LogMessageDto(uint ordinal, string module, Severity severity, string topic, string message, DateTime createdTime, params object[]? args)
        {
            Ordinal = ordinal;
            Module = module;
            Severity = severity;
            Topic = topic;
            Message = message;
            CreatedTime = createdTime;
            Args = args;
        }

        public string FormatMessageString()
            => Args is not null && Args.Length > 0 ? string.Format(Message, Args) : Message;

        public void MakeMessageStringFormatted()
        {
            if (Args is null)
            {
                return;
            }

            Message = FormatMessageString();
            Args = null;
        }

        public string Format() // todo: optional template string as a parameter
        {
            var builder = new StringBuilder();

            builder.Append($"[{CreatedTime}] {Ordinal} [{Severity}] [{Module}\\{Topic}] ");

            if (Args is not null)
            {
                builder.Append(FormatMessageString());
            }
            else
            {
                builder.Append(Message);
            }

            return builder.ToString();
        }

        public override bool Equals(object? obj)
        {
            return obj is LogMessageDto dto &&
                   Ordinal == dto.Ordinal &&
                   Module == dto.Module &&
                   Severity == dto.Severity &&
                   CreatedTime == dto.CreatedTime &&
                   Topic == dto.Topic &&
                   Message == dto.Message &&
                   EqualityComparer<object[]?>.Default.Equals(Args, dto.Args);
        }

        public override int GetHashCode()
            => HashCode.Combine(Ordinal, Module, Severity, CreatedTime, Topic, Message, Args);

        public byte[] ToByteArray()
            => JsonSerializer.SerializeToUtf8Bytes(this);

        public static LogMessageDto FromByteArray(byte[] array, uint offset, uint count)
            => FromByteArray(array, (int)offset, (int)count);

        public static LogMessageDto FromByteArray(byte[] array, int offset, int count)
            => JsonSerializer.Deserialize<LogMessageDto>(new ReadOnlySpan<byte>(array, offset, count))!;
    }
}