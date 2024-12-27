
namespace k3d.Logging.Interface
{
    public interface ILoggerCollection: IEnumerable<ILogger>
    {
        ILogger GetLogger(string module, string topic);
    }
}
