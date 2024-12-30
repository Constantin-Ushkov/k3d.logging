using System.Runtime.InteropServices;
using k3d.Logging.Interface;

// todo: format provider

namespace k3d.Logging.Impl
{
    public class ConsoleWriter : IOutputWriter
    {
        #region Dll Import

        //todo: isn't it windows specific code?
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool FreeConsole();

        #endregion // Dll Import

        #region Constructors/Finalizer

        public ConsoleWriter(bool allocateConsole)
        {
            if (allocateConsole)
            {
                AllocConsole();
            }
        }

        ~ConsoleWriter()
        {
            Dispose(false);
        }

        #endregion // Constructors/Finalizer

        #region IReportingListener Methods
        
        public void WriteMessage(LogMessageDto message)
        {
            Console.WriteLine($"[{message.CreatedTime}] {message.Severity} " +
                $"[{message.Module}{(string.IsNullOrEmpty(message.Topic) ? string.Empty : "\\" + message.Topic)}] " +
                $": {message.FormatMessageString()}");
        }

        #endregion // IReportingListener Methods

        #region IDisposable Methods

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                FreeConsole();
            }

            _disposed = true;
        }

        #endregion // IDisposable Methods

        #region Fields

        private bool _disposed;

        #endregion // Fields
    }
}
