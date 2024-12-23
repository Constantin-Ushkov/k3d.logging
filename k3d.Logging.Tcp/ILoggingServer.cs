namespace Ice.TcpLogger
{
    public interface ILoggingServer: IDisposable
    {
        IMessageHistory Messages { get; }

        void Start();
    }
}