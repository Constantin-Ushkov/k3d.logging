
namespace k3d.Logging.Interface
{
    public interface IFactory
    {
        IMessageDtoSerializer CreateMessageSerializer();
    }
}
