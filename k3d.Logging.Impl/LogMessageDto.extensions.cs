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
    }
}
