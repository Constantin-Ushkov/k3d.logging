namespace k3d.Logging.Tcp
{
    public interface ILoggingClientConfiguration
    {
        string? ServerHost { get; }
        uint ServerPort { get; }
        uint QueueSizeLimit { get; }
        uint SendAttemptCount { get; }
        TimeSpan SendErrorTimeout { get; }
    }
}