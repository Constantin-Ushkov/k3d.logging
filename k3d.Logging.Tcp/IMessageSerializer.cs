using k3d.Logging.Interface;

namespace k3d.Logging.Tcp
{
    internal interface IMessageSerializer
    {
        byte[] SerializeMessage(LogMessageDto message);
        LogMessageDto DeserializeMessage(byte[] bytes);
    }
}
