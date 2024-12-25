namespace k3d.Logging.Impl.Tcp
{
    public interface ILoggingServer: IDisposable
    {
        IMessageHistory Messages { get; }

        void Start();
    }
}