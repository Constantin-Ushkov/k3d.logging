
namespace k3d.Logging.Interface
{
    public interface IMessageDtoSerializer
    {
        LogMessageDto Deserialize(byte[] bytes, uint offset, uint count);
        LogMessageDto Deserialize(byte[] bytes, int offset, int count);

        byte[] Serialize(LogMessageDto message);
    }
}
