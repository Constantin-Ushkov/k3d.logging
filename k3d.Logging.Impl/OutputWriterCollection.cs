using k3d.Logging.Interface;
using System.Collections;

namespace k3d.Logging.Impl
{
    internal class OutputWriterCollection : IOutputWriterCollection
    {
        public void Add(IOutputWriter writer)
            => _writers.Add(writer);

        public bool Remove(IOutputWriter writer)
            => _writers.Remove(writer);

        public IOutputWriter AddConsoleWriter(bool allocateConsole)
        {
            var writer = new ConsoleWriter(allocateConsole);

            _writers.Add(writer);

            return writer;
        }

        public IOutputWriter AddFileWriter()
        {
            throw new NotImplementedException();
        }

        public IOutputWriter AddTcpWriter()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            // no UNmanaged resources, no need for generic dispose pattern
            _writers.ForEach(writer => writer.Dispose());
            _writers.Clear();
        }

        public IEnumerator<IOutputWriter> GetEnumerator()
            => _writers.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _writers.GetEnumerator();

        private readonly List<IOutputWriter> _writers = [];
    }
}
