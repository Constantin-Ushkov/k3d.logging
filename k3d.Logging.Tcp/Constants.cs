namespace k3d.Logging.Tcp
{
    public static class Constants
    {
        public const string ModuleName = "Ice.TcpLogging";
        public const string Topic = "Logging";

        public const int MinMessageLength = 12; // 7 - signature + 4 - length + 1 (at least) message body

        public static class Signatures
        {
            public const string Message = "<|MSG|>";
            public const string Acknowlegment = "<|ACK|>";
        }
    }
}