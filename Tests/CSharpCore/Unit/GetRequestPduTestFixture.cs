using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Lextm.SharpSnmpLib.Unit
{
    public class GetRequestPduTestFixture
    {
        [Fact]
        public void TestException()
        {
            Assert.Throws<ArgumentNullException>(() => new GetRequestPdu(0, new Span<byte>(new byte[] { 0 }), null));
            Assert.Throws<ArgumentNullException>(() => new GetRequestPdu(0, null));
            Assert.Throws<ArgumentNullException>(() => new GetRequestPdu(0, new List<Variable>()).AppendBytesTo(null));
#if !NETCOREAPP2_0
            Assert.Throws<ArgumentNullException>(() => new GetRequestPdu(null, new MemoryStream()));
#endif
        }

        [Fact]
        public void TestConstructor()
        {
            var pdu = new GetRequestPdu(0, new List<Variable>());
            Assert.Equal("GET request PDU: seq: 0; status: 0; index: 0; variable count: 0", pdu.ToString());
        }
    }
}
