using k3d.Logging.Interface;

namespace k3d.Logging.Test
{
    [TestClass]
    public sealed class LogMessageDtoTest
    {
        [TestMethod]
        public void TestSerialization()
        {
            var dto1 = new LogMessageDto(1, "mod1", Severity.Warning, "topic1", "msg {0} {1}", new[] { 1, 2 });

            dto1.MakeMessageStringFormatted();

            var bytes = dto1.ToByteArray();
            var dto2 = LogMessageDto.FromByteArray(bytes, 0, bytes.Length);

            Assert.IsNotNull(dto2);
            Assert.IsTrue(dto2.Equals(dto1));
        }
    }
}
