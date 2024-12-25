using k3d.Logging.Interface;
using k3d.Logging.Impl;

namespace k3d.Logging.Test
{
    [TestClass]
    public sealed class LogMessageDtoTest
    {
        [TestMethod]
        public void TestSerialization()
        {
            var serializer = new MessageDtoSerializer();

            var dto1 = new LogMessageDto(1, "mod1", Severity.Warning, "topic1", "msg {0} - {1} end", DateTime.Now, new object[] { 1, 2 });

            var bytes = serializer.Serialize(dto1);
            var dto2 = serializer.Deserialize(bytes, 0, bytes.Length);

            Assert.IsNotNull(dto2);
            Assert.IsTrue(dto2.Equals(dto1));
        }
    }
}
