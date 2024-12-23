
namespace k3d.Logging.Interface
{
    public interface ILoggingListener : IDisposable
    {
        void Message(LogMessageDto message);
    }
}
