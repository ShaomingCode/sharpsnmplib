using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Lextm.SharpSnmpLib.Unit
{
    public class StreamExtensionTestFixture
    {
        [Fact]
        public async Task TestException()
        {
            Assert.Throws<ArgumentNullException>(() => StreamExtension.AppendBytes(null, SnmpType.Counter32, null, null));
            Assert.Throws<ArgumentNullException>(() => StreamExtension.IgnoreBytes(null, 0));
#if NETCOREAPP2_0
            var length = 0;
            await Assert.ThrowsAsync<ArgumentNullException>(async () => StreamExtension.ReadPayloadLength(null, out length));
#else
            Assert.Throws<ArgumentNullException>(() => new MemoryStream().AppendBytes(SnmpType.Counter32, null, null));
            Assert.Throws<ArgumentNullException>(() => StreamExtension.ReadPayloadLength(null));
#endif
        }
    }
}
