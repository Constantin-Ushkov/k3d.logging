namespace k3d.Logging.Tcp
{
    public interface ILoggingServer: IDisposable
    {
        IMessageHistory Messages { get; }

        void Start();
    }
}