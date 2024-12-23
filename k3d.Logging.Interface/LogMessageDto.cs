using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

// todo: move serialization logic out of this class

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

        public LogMessageDto(uint ordinal, string module, Severity severity, string topic, string message, params object[]? args)
        {
            Ordinal = ordinal;
            Module = module;
            Severity = severity;
            Topic = topic;
            Message = message;
            Args = args;
            CreatedTime = DateTime.Now;
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

        public byte[] ToByteArray()
        {
            // https://metanit.com/sharp/tutorial/6.2.php
            // todo: _core.Serialization.Binary.Serialize()
            
            using var memory = new MemoryStream();
            var formatter = new BinaryFormatter();
            
            formatter.Serialize(memory, this);
            return memory.ToArray();
        }

        public static LogMessageDto FromByteArray(byte[] array, uint offset, uint count)
        {
            using var memory = new MemoryStream();
            
            memory.Write(array, (int)offset, (int)count);
            memory.Seek(0, SeekOrigin.Begin);
            
            return new BinaryFormatter().Deserialize(memory) as LogMessageDto;
        }
        
        public static LogMessageDto FromByteArray(byte[] array, int offset, int count)
        {
            using var memory = new MemoryStream();
            
            memory.Write(array, offset, count);
            memory.Seek(0, SeekOrigin.Begin);
            
            return new BinaryFormatter().Deserialize(memory) as LogMessageDto;
        }
    }
}