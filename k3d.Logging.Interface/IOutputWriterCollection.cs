
namespace k3d.Logging.Interface
{
    public interface IOutputWriterCollection: IEnumerable<IOutputWriter>, IDisposable
    {
        void Add(IOutputWriter writer);
        bool Remove(IOutputWriter writer);

        IOutputWriter AddConsoleWriter(bool allocateConsole);
        IOutputWriter AddFileWriter(string file, bool overwrite, IMessageFormatter? formatter = null);
        IOutputWriter AddTcpWriter();
    }
}
