using k3d.Logging.Interface;

namespace k3d.Logging.Impl
{
    public class Factory: IFactory
    {
        public IMessageDtoSerializer CreateMessageSerializer()
            => new MessageDtoSerializer();

        public static ILoggingService CreateLoggingService()
            => new LoggingService();
    }
}
