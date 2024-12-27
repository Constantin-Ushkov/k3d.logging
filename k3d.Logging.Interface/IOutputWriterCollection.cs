
namespace k3d.Logging.Interface
{
    public interface IOutputWriterCollection: IEnumerable<IOutputWriter>
    {
        void Add(IOutputWriter writer);

        IOutputWriter AddConsoleWriter();
        IOutputWriter AddFileWriter();
        IOutputWriter AddTcpWriter();
    }
}
