namespace k3d.Logging.Impl.Tcp
{
    public class LoggingClientConfiguration: ILoggingClientConfiguration
    {
        public string? ServerHost { get; set; }
        public uint ServerPort { get; set; }
        public uint QueueSizeLimit { get; set; }
        public uint SendAttemptCount { get; set; }
        public TimeSpan SendErrorTimeout { get; set; }
    }
}