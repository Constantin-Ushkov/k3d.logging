using k3d.Logging.Interface;

namespace k3d.Logging.Impl.Tcp
{
    internal interface IMessageSerializer
    {
        byte[] SerializeMessage(LogMessageDto message);
        LogMessageDto DeserializeMessage(byte[] bytes);
    }
}
