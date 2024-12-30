using k3d.Common.Diagnostics;
using k3d.Logging.Interface;

namespace k3d.Logging.Impl
{
    internal class FileWriter : IOutputWriter
    {
        public FileWriter(string file, bool overwrite, IMessageFormatter formatter)
        {
            Assert.Argument.IsNotNullOrEmpty(file, nameof(file));
            Assert.Argument.IsNotNull(formatter, nameof(formatter));

            _formatter = formatter;

            if (File.Exists(file))
            {
                _writer = new StreamWriter(file, !overwrite);
            }
            else
            {
                _writer = new StreamWriter(file);
            }
        }

        public void WriteMessage(LogMessageDto message)
        {
            _writer.WriteLine(_formatter.Format(message));
            _writer.Flush();
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _writer.Dispose();
            _disposed = true;
        }

        private bool _disposed;
        private IMessageFormatter _formatter;
        private StreamWriter _writer;
    }
}
