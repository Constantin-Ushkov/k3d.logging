
namespace k3d.Logging.Interface
{
    public interface IOutputWriter : IDisposable
    {
        void WriteMessage(LogMessageDto message);
    }
}
