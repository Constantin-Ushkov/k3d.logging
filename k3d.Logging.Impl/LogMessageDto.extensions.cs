using System.Text;
using System.Text.Json;
using k3d.Logging.Interface;

namespace k3d.Logging.Impl
{
    internal static class LogMessageDtoExtensions
    {
        public static string FormatMessageString(this LogMessageDto dto)
            => dto.Args is not null && dto.Args.Length > 0 ? string.Format(dto.Message, dto.Args) : dto.Message;

        public static void MakeMessageStringFormatted(this LogMessageDto dto)
        {
            if (dto.Args is null)
            {
                return;
            }

            dto.Message = dto.FormatMessageString();
            dto.Args = null;
        }

        public static string Format(this LogMessageDto dto) // todo: optional template string as a parameter
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
