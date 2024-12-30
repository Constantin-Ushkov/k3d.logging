using k3d.Logging.Interface;
using System.Text;

namespace k3d.Logging.Impl
{
    internal class MessageFormatter : IMessageFormatter
    {
        public string Format(LogMessageDto dto)
        {
            var builder = new StringBuilder();

            builder.Append($"[{dto.CreatedTime}] {dto.Ordinal} [{dto.Severity}] [{dto.Module}\\{dto.Topic}] ");

            if (dto.Args is not null)
            {
                builder.Append(dto.FormatMessageString());
            }
            else
            {
                builder.Append(dto.Message);
            }

            return builder.ToString();
        }
    }
}
