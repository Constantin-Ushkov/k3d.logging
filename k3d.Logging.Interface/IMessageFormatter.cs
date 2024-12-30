
namespace k3d.Logging.Interface
{
    public interface IMessageFormatter
    {
        string Format(LogMessageDto message);
    }
}
