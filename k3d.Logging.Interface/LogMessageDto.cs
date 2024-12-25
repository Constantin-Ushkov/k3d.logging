
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
        public string Message { get; set; }
        public object[]? Args { get; set; }

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
    }
}